using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Errors;

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
            
            public async Task<List<Audit>> Handle(Query request, CancellationToken cancellationToken)
            {
                var audits = await _auditReader.ReadAudit(cancellationToken, new Audit
                {
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    Username = request.Username
                },true);

                if (audits.Count == 0)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Audits = Constants.NOT_FOUND});
                }

                return audits;
            }
        }
    }
}