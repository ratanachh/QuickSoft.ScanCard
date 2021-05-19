using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard.Features.Cards
{
    public static class List
    {
        public record Query : IRequest<List<Card>>;
        
        public class Handler : IRequestHandler<Query, List<Card>>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            public async Task<List<Card>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Cards.Select(c =>new Card
                {
                    CardNumber = c.CardNumber,
                    CreatedDate = c.CreatedDate,
                    IsActive = c.IsActive,
                    AuditId = c.AuditId,
                    CustomerId = c.CustomerId
                }).ToListAsync(cancellationToken);
            }
        }
    }
}