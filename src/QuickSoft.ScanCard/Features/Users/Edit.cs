using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Security;

namespace QuickSoft.ScanCard.Features.Users
{
    public class Edit
    {
        public class UserData
        {
            public string Username { get; set; }
            public string ProfileUrl { get; set; }
            public string Phone { get; set; }
            public string Password { get; set; }

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
            private readonly ApplicationDbContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IPasswordHasher passwordHasher,
                ICurrentUserAccessor currentUserAccessor, IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _currentUserAccessor = currentUserAccessor;
                _mapper = mapper;
            }
            public async Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var currentUserName = _currentUserAccessor.GetCurrentUsername();
                var person = await _context.Users.Where(x => x.Username == currentUserName)
                    .FirstOrDefaultAsync(cancellationToken);

                person.Username = request.User.Username ?? person.Username;
                person.ProfileUrl = request.User.ProfileUrl ?? person.ProfileUrl;
                person.Phone = request.User.Phone ?? person.Phone;
                person.UserType = request.User.UserType == 0 ? person.UserType : request.User.UserType;

                if (!string.IsNullOrWhiteSpace(request.User.Password))
                {
                    person.Password = _passwordHasher.Hash(request.User.Password);
                }

                await _context.SaveChangesAsync(cancellationToken);

                return new UserEnvelope(_mapper.Map<Domain.User, User>(person));
            }
        }
    }
}