namespace QuickSoft.ScanCard.Features.Audits.Queries
{
    using System.Linq;
    using Infrastructure;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

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
                return await 
                    _context.Audits
                    .SelectMany( 
                        au => _context.Persons
                            .Select(p => new {p.Id, p.Username})
                            .Where(x => x.Id == au.PersonId),
                        (au, ps) => new Audit
                        {
                            Id = au.Id,
                            Descriptions = au.Descriptions,
                            Username = ps.Username,
                            CreatedDate = au.CreatedDate
                        })
                    .ToListAsync(cancellationToken);
            }

            return await 
                _context.Audits
                .SelectMany(
                    au => _context.Persons
                        .Select(p => new {p.Id, p.Username})
                        .Where(x => x.Id == au.PersonId),
                    (au, ps) => new {au, ps})
                .Where(t =>
                    (audit.Username.Trim().Equals(string.Empty) || t.ps.Username.Contains(audit.Username))
                    && t.au.CreatedDate.Date >= audit.FromDate.Date && t.au.CreatedDate.Date <= audit.ToDate.Date)
                .AsNoTracking()
                .Select(t => new Audit
                {
                    Id = t.au.Id,
                    Descriptions = t.au.Descriptions,
                    Username = t.ps.Username,
                    CreatedDate = t.au.CreatedDate
                }).ToListAsync(cancellationToken);
        }
    }
}