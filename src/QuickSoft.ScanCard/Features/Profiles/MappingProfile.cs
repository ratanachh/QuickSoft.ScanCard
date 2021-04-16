using AutoMapper;
using QuickSoft.ScanCard.Domain;

namespace QuickSoft.ScanCard.Features.Profiles
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, Profile>(MemberList.None);
        }
    }
}