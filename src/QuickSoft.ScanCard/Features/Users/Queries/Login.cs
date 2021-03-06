using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Domain;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Errors;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard.Features.Users.Queries
{
    public static class Login
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
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private const int ValidPeriodUser = 86400;

            public Handler(ApplicationDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
                _currentUserAccessor = currentUserAccessor;
            }
            public async Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var person = await _context.Persons
                    .Where(x => x.Username == request.User.Username)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(cancellationToken);
                if (person == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }

                if (!_passwordHasher.Verify(request.User.Password, person.Password))
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                }

                var user = _mapper.Map<Person, User>(person);
                user.Type = UserConstants.GetUserTypeString(person.UserType);
                user.IsCurrentUser = true;
                
                /*
                 * Trace user login
                 */
                var userAgent = _currentUserAccessor.GetUserAgent();
                var description = $"LoggedIn using ip: {_currentUserAccessor.GetUserIp()}, " +
                                  $"UserAgent: {userAgent.OS} {userAgent.Name} {userAgent.Version}";
                var audit = new Audit()
                {
                    Descriptions = description,
                    CreatedDate = DateTime.Now,
                    PersonId = person.Id
                };
                await _context.Audits.AddAsync(audit, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                user.Token = _jwtTokenGenerator.ValidTokenTime(ValidPeriodUser).CreateToken(person.Username, user.Type, audit.Id.ToString());
                
                return new UserEnvelope(user);
            }
        }
    }
}