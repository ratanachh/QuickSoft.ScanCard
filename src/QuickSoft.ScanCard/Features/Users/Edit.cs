using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace QuickSoft.ScanCard.Features.Users
{
    public class Edit
    {
        public class UserData
        {
            public string Username { get; set; }
        
            public string ProfileUrl { get; set; }

            public string Phone { get; set; }

            public int UserType { get; set; }
        }
        
        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull();
            }
        }
        
        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            
            public Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}