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
    public static class Edit
    {
        public class Command : IRequest<CustomerEnvelope>
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string CardNumber { get; set; }
            public bool DisableCard { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Name).NotEmpty().NotNull();
                RuleFor(x => x.Phone).NotEmpty().NotNull();
                RuleFor(x => x.CardNumber).NotEmpty().NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, CustomerEnvelope>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CustomerEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var card = await _context.Cards
                    .FirstOrDefaultAsync(x => x.CardNumber == request.CardNumber, cancellationToken);
                if (card == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new {Card = "The card is not exist"});
                }

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                
                if (card.CustomerId != 0 && customer.Id != card.CustomerId)
                {
                    throw new RestException(HttpStatusCode.NotFound, new {Card = "The card is already used."});
                }

                if (card.CardNumber != request.CardNumber)
                {
                    // To find current customer card
                    var currentCard = await _context.Cards.FirstOrDefaultAsync(x => x.CustomerId == customer.Id && x.IsActive, cancellationToken);
                    if (currentCard != null)
                    {
                        currentCard.IsActive = false;
                    }
                }

                customer.Name = request.Name ?? customer.Name;
                customer.Phone = request.Phone ?? customer.Phone;
                card.IsActive = !request.DisableCard;
                card.CustomerId = customer.Id;
                await _context.SaveChangesAsync(cancellationToken);

                return new CustomerEnvelope(new Customer
                {
                    Id = customer.Id,
                    AuditId = customer.AuditId,
                    CardNumber = card.CardNumber,
                    CreatedDate = customer.CreatedDate,
                    IsCardActive = card.IsActive,
                    Name = customer.Name,
                    Phone = customer.Phone
                });
            }
        }
    }
}