using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Domain;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Errors;
using QuickSoft.ScanCard.Infrastructure.Security;
using Shyjus.BrowserDetection;
using Shyjus.BrowserDetection.Browsers;

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
            private readonly ICurrentUserAccessor _currentUserAccessor;

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
                var person = await _context.Persons.Where(x => x.Username == request.User.Username).SingleOrDefaultAsync(cancellationToken);
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
                user.Token = _jwtTokenGenerator.ValidTokenTime(86400).CreateToken(person.Username, user.Type);
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
                
                
                return new UserEnvelope(user);
            }
        }
    }
}