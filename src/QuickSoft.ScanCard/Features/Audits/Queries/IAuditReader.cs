namespace QuickSoft.ScanCard.Features.Audits.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    public interface IAuditReader
    {
        Task<List<Audit>> ReadAudit(CancellationToken cancellationToken, Audit audit = null, bool isSearch = false);
    }
}