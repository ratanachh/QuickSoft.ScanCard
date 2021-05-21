using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuickSoft.ScanCard.Features.Cards.Commands;
using QuickSoft.ScanCard.Features.Cards.Queries;

namespace QuickSoft.ScanCard.Features.Cards
{
    [ApiController]
    [Route("card")]
    // [Authorize]
    public class CardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CardController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("create")]
        public Task<CardEnvelope> Create([FromBody] Create.Command command, CancellationToken cancellationToken)
        {
            return _mediator.Send(command, cancellationToken);
        }

        [HttpPost("search")]
        public Task<Domain.Card> SelectByCardNumber([FromBody]Search.Query query, CancellationToken cancellationToken)
        {
            return _mediator.Send(query, cancellationToken);
        }
    }
}