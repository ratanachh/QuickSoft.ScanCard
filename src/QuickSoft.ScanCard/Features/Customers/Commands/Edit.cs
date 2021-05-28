using FluentValidation;
using MediatR;

namespace QuickSoft.ScanCard.Features.Customers.Commands
{
    public static class Edit
    {
        public class Command : IRequest<CustomerEnvelope>
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string CardNumber { get; set; }
            public bool DisableCard { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty().NotNull();
                RuleFor(x => x.Phone).NotEmpty().NotNull();
                RuleFor(x => x.CardNumber).NotEmpty().NotNull();
                RuleFor(x => x.DisableCard).NotEmpty().NotNull();
            }
        }
        
        
        
    }
}