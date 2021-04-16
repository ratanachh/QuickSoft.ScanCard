
using AutoMapper;

namespace QuickSoft.ScanCard.Features.Users
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Person, User>(MemberList.None);
        }
    }
}