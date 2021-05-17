using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public Task<List<Card>> List(CancellationToken cancellationToken)
        {
            return _mediator.Send(new List.Query(), cancellationToken);
        }
    }
}