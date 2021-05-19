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

namespace QuickSoft.ScanCard.Features.Profiles
{
    public static class Details
    {
        public class Query : IRequest<ProfileEnvelope>
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
        
        public class QueryHandler : IRequestHandler<Query, ProfileEnvelope>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            public async Task<ProfileEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                var person = await _context.Persons.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

                if (person == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new {User = Constants.NOT_FOUND});
                }
                var profile = _mapper.Map<Person, Profile>(person);
                return new ProfileEnvelope(profile);
            }
        }
        
    }
}