using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace QuickSoft.ScanCard.Features.Users
{
    public class Create
    {
        public class UserData
        {
            public string Username { get; set; }
        
            public string ProfileUrl { get; set; }

            public string Phone { get; set; }

            public int UserType { get; set; }
            
            public string Password { get; set; }
        }

        class UserDataValidation : AbstractValidator<UserData>
        {
            public UserDataValidation()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
                RuleFor(x => x.Phone).NotNull().NotEmpty();
                RuleFor(x => x.UserType).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }
        
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.User).NotNull().SetValidator(new UserDataValidation());
            }
        }
        
        public class CommandHandler : IRequestHandler<Command, UserEnvelope>
        {
            public Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }
        }
        
    }
}