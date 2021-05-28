using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ScanCard.Features.Customers.Queries;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard.Features.Customers
{
    [ApiController]
    [Route("customers")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public Task<CustomersEnvelope> Get(CancellationToken cancellationToken)
        {
            return _mediator.Send(new List.Query(), cancellationToken);
        }
    }
}