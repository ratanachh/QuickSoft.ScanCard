using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuickSoft.ScanCard.Features.Cards
{
    public interface ICardReader
    {
        Task<List<Card>> ReadAudit(Card card, CancellationToken cancellationToken, bool isSearch = false);
    }
}