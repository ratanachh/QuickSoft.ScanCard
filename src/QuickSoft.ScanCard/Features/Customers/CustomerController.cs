using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ScanCard.Features.Customers.Commands;

namespace QuickSoft.ScanCard.Features.Customers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public Task<CustomerEnvelope> Create([FromBody]Create.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }
    }
}