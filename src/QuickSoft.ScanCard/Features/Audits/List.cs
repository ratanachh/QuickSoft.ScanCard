using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace QuickSoft.ScanCard.Features.Audits
{
    public static class List
    {
        public record Query : IRequest<List<Audit>>;
        
        public class Handler : IRequestHandler<Query, List<Audit>>
        {
            private readonly IAuditReader _auditReader;

            public Handler(IAuditReader auditReader)
            {
                _auditReader = auditReader;
            }
            public Task<List<Audit>> Handle(Query request, CancellationToken cancellationToken)
            {
                return _auditReader.ReadAudit(cancellationToken);
            }
        }
    }
}