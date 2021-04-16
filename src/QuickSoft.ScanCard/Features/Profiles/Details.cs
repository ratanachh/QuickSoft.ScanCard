using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace QuickSoft.ScanCard.Features.Profiles
{
    public class Details
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
            private readonly IProfileReader _profileReader;

            public QueryHandler(IProfileReader profileReader)
            {
                _profileReader = profileReader;
            }
            public Task<ProfileEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                return _profileReader.ReadProfile(request.Username, cancellationToken);
            }
        }
        
    }
}