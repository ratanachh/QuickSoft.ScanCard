using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace QuickSoft.ScanCard.Features.Customers.Commands
{
    public static class Create
    {
        public class Command : IRequest<Customer>
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string CardNumber { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().NotNull();
                RuleFor(x => x.Phone).NotEmpty().NotNull();
                RuleFor(x => x.CardNumber).NotEmpty().NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Command, Customer>
        {
            public Task<Customer> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}