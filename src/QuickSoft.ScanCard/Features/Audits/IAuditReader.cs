using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuickSoft.ScanCard.Features.Audits
{
    public interface IAuditReader
    {
        Task<List<Audit>> ReadAudit(Audit audit, CancellationToken cancellationToken, bool isSearch = false);
    }
}