using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Errors;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard.Features.Users
{
    public class Login
    {
        public class UserData
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
        
        public class UserValidation : AbstractValidator<UserData>
        {
            public UserValidation()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }
        
        public class Command : IRequest<UserEnvelope>
        {
            public UserData User { get; set; }
        }
        
        public class CommandValidation : AbstractValidator<Command>
        {
            public CommandValidation()
            {
                RuleFor(x => x.User).NotNull().SetValidator(new UserValidation());
            }
        }
        
        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly ApplicationDbContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
            }
            public async Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var person = await _context.Persons.Where(x => x.Username == request.User.Username).SingleOrDefaultAsync(cancellationToken);
                if (person == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new {Error = "Invalid email / password."});
                }

                if (!person.Password.SequenceEqual(_passwordHasher.Hash(request.User.Password)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new {Error = "Invalid email / password."});
                }

                var user = _mapper.Map<Domain.Person, User>(person);
                user.Token = _jwtTokenGenerator.CreateToken(person.Username);
                return new UserEnvelope(user);
            }
        }
    }
}