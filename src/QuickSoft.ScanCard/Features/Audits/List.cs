using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Domain;
using QuickSoft.ScanCard.Infrastructure;

namespace QuickSoft.ScanCard.Features.Audits
{
    public class List
    {
        public class Query : IRequest<List<Audit>>
        {
        }
        
        public class Handler : IRequestHandler<Query, List<Audit>>
        {
            private readonly IAuditReader _auditReader;

            public Handler(IAuditReader auditReader)
            {
                _auditReader = auditReader;
            }
            public Task<List<Audit>> Handle(Query request, CancellationToken cancellationToken)
            {
                return _auditReader.ReadAudit(null, cancellationToken);
            }
        }
    }
}