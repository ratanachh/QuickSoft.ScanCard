namespace QuickSoft.ScanCard.Features.Audits
{
    using Queries;
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using Infrastructure.Security;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;

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