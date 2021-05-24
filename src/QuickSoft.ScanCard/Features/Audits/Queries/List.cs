namespace QuickSoft.ScanCard.Features.Audits.Queries
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;

    public static class List
    {
        public record Query : IRequest<AuditEnvelope>;
        
        public class Handler : IRequestHandler<Query, AuditEnvelope>
        {
            private readonly IAuditReader _auditReader;

            public Handler(IAuditReader auditReader)
            {
                _auditReader = auditReader;
            }
            public async Task<AuditEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var audits = await _auditReader.ReadAudit(cancellationToken);
                return new AuditEnvelope(audits);
            }
        }
    }
}