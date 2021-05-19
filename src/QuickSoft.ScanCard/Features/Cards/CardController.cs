using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace QuickSoft.ScanCard.Features.Cards
{
    [ApiController]
    [Route("card")]
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

        [HttpGet("{cardNumber}")]
        public Task<Domain.Card> SelectByCardNumber(string cardNumber, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Select.ByCardNumber.Query
            {
                CardNumber = cardNumber
            }, cancellationToken);
        }
    }
}