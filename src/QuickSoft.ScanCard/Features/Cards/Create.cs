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

namespace QuickSoft.ScanCard.Features.Cards
{
    public class Create
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

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<CardEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                if (await _context.Cards.Where(x => x.CardNumber == request.CardNumber).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { CardNumber = Constants.IN_USE });
                }

                var cardData = new Domain.Card()
                {
                    CardNumber = request.CardNumber,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };
                await _context.Cards.AddAsync(cardData, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                var card = _mapper.Map<Domain.Card, Card>(cardData);

                return new CardEnvelope(card);
            }
        }
    }
}