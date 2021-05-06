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
using Constants = QuickSoft.ScanCard.Infrastructure.Errors.Constants;

namespace QuickSoft.ScanCard.Features.Users
{
    public class Details
    {
        public class Query : IRequest<UserEnvelope>
        {
            public string Username { get; set; }
        }
        
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
            }
        }
        
        public class Handler : IRequestHandler<Query, UserEnvelope>
        {
            private readonly ApplicationDbContext _context;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(ApplicationDbContext context, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<UserEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var currentUsername = _currentUserAccessor.GetCurrentUsername();
                var person = await _context.Persons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

                if (person == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { User = Constants.NOT_FOUND });
                }
                
                if (_currentUserAccessor.GetCurrentUserType().Equals(UserConstants.User)
                    && !currentUsername.Equals(person.Username)
                )
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { User = Constants.UNAUTHERIZE });
                }

                var user = _mapper.Map<Person, User>(person);
                user.Type = UserConstants.GetUserTypeString(person.UserType);
                
                if (!user.Username.Equals(currentUsername)) return new UserEnvelope(user);
                // To mark the profile UI is current user
                user.IsCurrentUser = true;
                return new UserEnvelope(user);
            }
        }
    }
}