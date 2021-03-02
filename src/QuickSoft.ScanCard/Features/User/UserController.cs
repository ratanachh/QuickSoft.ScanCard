using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard.Features.User
{
    [Route("user")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public class UserController
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        public Task<UserEnvelope> GetCurrent(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}