namespace QuickSoft.ScanCard.Features.Audits.Queries
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using QuickSoft.ScanCard.Infrastructure;
    public class AuditReader : IAuditReader
    {
        private readonly ApplicationDbContext _context;

        public AuditReader(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Audit>> ReadAudit(CancellationToken cancellationToken, Audit audit = null, bool isSearch = false)
        {
            if (!isSearch)
            {
                return await (from au in _context.Audits
                        from ps in _context.Persons
                            .Select(p => new {p.Id, p.Username})
                            .Where(x => x.Id == au.PersonId)
                        select new Audit
                        {
                            Id = au.Id, Descriptions = au.Descriptions, Username = ps.Username,
                            CreatedDate = au.CreatedDate
                        }
                    ).ToListAsync(cancellationToken);
            }

            return await (from au in _context.Audits
                    from ps in _context.Persons
                        .Select(p => new {p.Id, p.Username})
                        .Where(x => x.Id == au.PersonId)
                    where (audit.Username.Trim().Equals(string.Empty) || ps.Username.Contains(audit.Username)) 
                          && (au.CreatedDate.Date >= audit.FromDate.Date && au.CreatedDate.Date <= audit.ToDate.Date)
                    select new Audit
                    {
                        Id = au.Id, Descriptions = au.Descriptions, Username = ps.Username,
                        CreatedDate = au.CreatedDate
                    }
                ).ToListAsync(cancellationToken);
        }
    }
}