using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard.Features.Cards.Queries
{
    public class SearchByCardNumber
    {
        public class Query : IRequest<Domain.Card>
        {
            public string CardNumber { get; set; }
        }
        
        public class Handler : IRequestHandler<Query, Domain.Card>
        {
            private readonly ApplicationDbContext _context;

            public Handler(ApplicationDbContext context)
            {
                _context = context;
            }
            
            public async Task<Domain.Card> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Cards.FirstOrDefaultAsync(c => c.CardNumber == request.CardNumber, cancellationToken);
            }
        }
    }
}