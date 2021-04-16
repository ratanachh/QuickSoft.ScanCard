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
using Constants = QuickSoft.ScanCard.Infrastructure.Errors.Constants;

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
                if (await _context.Persons.Where(x => x.Username == request.User.Username).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Username = Constants.IN_USE });
                }
                
                var person = new Person()
                {
                    Username = request.User.Username,
                    ProfileUrl = request.User.ProfileUrl,
                    Phone = request.User.Phone,
                    UserType = request.User.UserType,
                    Password = _passwordHasher.Hash(request.User.Password),
                };
                await _context.Persons.AddAsync(person, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                var user = _mapper.Map<Person, User>(person);
                user.Token = _jwtTokenGenerator.CreateToken(user.Username);
                return new UserEnvelope(user);
            }
        }
        
    }
}