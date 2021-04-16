using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard.Features.Profiles
{
    [Route("profiles")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public class ProfilesController
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{username}")]
        public Task<ProfileEnvelope> Get(string username, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Details.Query()
            {
                Username = username
            }, cancellationToken);
        }
    }
}