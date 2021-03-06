using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ScanCard.Features.Cards.Queries;

namespace QuickSoft.ScanCard.Features.Cards
{
    [ApiController]
    [Route("cards")]
    public class CardsController
    {
        private readonly IMediator _mediator;

        public CardsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<CardsEnvelope> List(CancellationToken cancellationToken)
        {
            return _mediator.Send(new List.Query(), cancellationToken);
        }
    }
}