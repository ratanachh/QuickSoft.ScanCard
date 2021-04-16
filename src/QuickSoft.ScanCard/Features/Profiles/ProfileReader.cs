using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuickSoft.ScanCard.Domain;
using QuickSoft.ScanCard.Infrastructure;
using QuickSoft.ScanCard.Infrastructure.Errors;

namespace QuickSoft.ScanCard.Features.Profiles
{
    public class ProfileReader : IProfileReader
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public ProfileReader(ApplicationDbContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
            _mapper = mapper;
        }
        public async Task<ProfileEnvelope> ReadProfile(string username, CancellationToken cancellationToken)
        {
            var currentUsername = _currentUserAccessor.GetCurrentUsername();

            var person = await _context.Persons.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);

            if (person == null)
            {
                throw new RestException(HttpStatusCode.NotFound, new {User = Constants.NOT_FOUND});
            }

            var profile = _mapper.Map<Person, Profile>(person);
            
            if (profile == null) return new ProfileEnvelope(null);
            
            if (profile.Username.Equals(currentUsername))
            {
                profile.IsCurrentUser = true;
            }

            profile.Type = person.UserType switch
            {
                0 => Constants.Admin,
                1 => Constants.Developer,
                2 => Constants.User,
                _ => profile.Type
            };

            return new ProfileEnvelope(profile);
        }
    }
}