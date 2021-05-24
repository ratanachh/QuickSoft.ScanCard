using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard.Features.Cards.Queries
{
    public static class List
    {
        public record Query : IRequest<CardsEnvelope>;
        
        public class Handler : IRequestHandler<Query, CardsEnvelope>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<CardsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var cards = await _context.Cards.Select(c =>new Card
                {
                    CardNumber = c.CardNumber,
                    CreatedDate = c.CreatedDate,
                    IsActive = c.IsActive,
                    AuditId = c.AuditId,
                    CustomerId = c.CustomerId
                }).ToListAsync(cancellationToken);

                return new CardsEnvelope(cards);
            }
        }
    }
}