namespace QuickSoft.ScanCard.Features.Audits.Queries
{
    using System;
    using MediatR;
    using System.Net;
    using Infrastructure;
    using System.Threading;
    using Infrastructure.Errors;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    public static class Search
    {
        public class Query : IRequest<AuditEnvelope>
        {
            public string Username { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            
        }
        
        public class Handler : IRequestHandler<Query, AuditEnvelope>
        {
            private readonly ApplicationDbContext _context;
            private readonly IAuditReader _auditReader;

            public Handler(ApplicationDbContext context, IAuditReader auditReader)
            {
                _context = context;
                _auditReader = auditReader;
            }
            
            public async Task<AuditEnvelope> Handle(Query request, CancellationToken cancellationToken)
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

                return new AuditEnvelope(audits);
            }
        }
    }
}