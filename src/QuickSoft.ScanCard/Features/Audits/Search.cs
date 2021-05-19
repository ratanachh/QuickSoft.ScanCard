using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard.Features.Audits
{
    public static class Search
    {
        public class Query : IRequest<List<Audit>>
        {
            public string Username { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            
        }
        
        public class Handler : IRequestHandler<Query, List<Audit>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IAuditReader _auditReader;

            public Handler(ApplicationDbContext context, IAuditReader auditReader)
            {
                _context = context;
                _auditReader = auditReader;
            }
            
            public Task<List<Audit>> Handle(Query request, CancellationToken cancellationToken)
            {
                return _auditReader.ReadAudit(cancellationToken, new Audit
                {
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    Username = request.Username
                },true);
            }
        }
    }
}