using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard.Features.Audits
{
    [ApiController]
    [Route("audits")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public class AuditsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuditsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public Task<List<Audit>> GetDetail(CancellationToken cancellationToken)
        {
            return _mediator.Send(new List.Query(), cancellationToken);
        }

        [HttpPost("search")]
        public Task<List<Audit>> Search([FromBody] Search.Query query, CancellationToken cancellationToken)
        {
            return _mediator.Send(query, cancellationToken);
        }
    }
}