using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ScanCard.Features.Customers.Queries;

namespace QuickSoft.ScanCard.Features.Customers
{
    [ApiController]
    [Route("customers")]
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