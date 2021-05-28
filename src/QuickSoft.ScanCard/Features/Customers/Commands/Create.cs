using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Errors;

namespace QuickSoft.ScanCard.Features.Customers.Commands
{
    public static class Create
    {
        public class Command : IRequest<CustomerEnvelope>
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string CardNumber { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().NotNull();
                RuleFor(x => x.Phone).NotEmpty().NotNull();
                RuleFor(x => x.CardNumber).NotEmpty().NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, CustomerEnvelope>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(ApplicationDbContext context, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _mapper = mapper;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<CustomerEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                #region Checking card

                var card = await _context.Cards
                    .FirstOrDefaultAsync(c => c.CardNumber == request.CardNumber, cancellationToken);

                if (card == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new {Customer = "The card is not exist"});
                }

                if (card.CustomerId != 0)
                {
                    throw new RestException(HttpStatusCode.NotFound, new {Customer = "The card is already used."});
                }

                #endregion

                // Insert customer
                var auditId = _currentUserAccessor.GetAuditId();
                var customer = new Domain.Customer
                {
                    AuditId = auditId,
                    CreatedDate = DateTime.Now,
                    Name = request.Name,
                    Phone = request.Phone,
                };
                await _context.Customers.AddAsync(customer, cancellationToken);
                // get id customer from database
                await _context.SaveChangesAsync(cancellationToken);

                // Updating card
                card.CustomerId = customer.Id;
                _context.Update(card);
                await _context.SaveChangesAsync(cancellationToken);

                var response = _mapper.Map<Domain.Customer, Customer>(customer);
                response.IsCardActive = card.IsActive;
                return new CustomerEnvelope(response);
            }
        }
    }
}