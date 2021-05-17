using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuickSoft.ScanCard.Features.Cards
{
    public class CardReader : ICardReader
    {
        public Task<List<Card>> ReadAudit(Card card, CancellationToken cancellationToken, bool isSearch = false)
        {
            throw new System.NotImplementedException();
        }
    }
}