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

namespace QuickSoft.ScanCard.Features.Cards.Commands
{
    public static class Create
    {
        public class Command : IRequest<CardEnvelope>
        {
            public string CardNumber { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.CardNumber).NotNull().NotEmpty();
            }
        }
        
        
        public class Handler : IRequestHandler<Command, CardEnvelope>
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
            public async Task<CardEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Cards.Where(x => x.CardNumber == request.CardNumber).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Card = Constants.ALREADY_EXIST });
                }
                var auditId = _currentUserAccessor.GetAuditId();

                var cardData = new Domain.Card
                {
                    CardNumber = request.CardNumber,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    AuditId = auditId
                };
                await _context.Cards.AddAsync(cardData, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                var card = _mapper.Map<Domain.Card, Card>(cardData);

                return new CardEnvelope(card);
            }
        }
    }
}