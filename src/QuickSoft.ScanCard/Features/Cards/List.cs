using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace QuickSoft.ScanCard.Features.Cards
{
    public class List
    {
        public class Query : IRequest<List<Card>>
        {
        }
        
        public class Handler : IRequestHandler<Query, List<Card>>
        {
            private readonly ICardReader _cardReader;

            public Handler(ICardReader cardReader)
            {
                _cardReader = cardReader;
            }
            public Task<List<Card>> Handle(Query request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}