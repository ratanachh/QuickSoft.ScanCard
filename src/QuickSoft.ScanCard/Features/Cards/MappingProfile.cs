using AutoMapper;

namespace QuickSoft.ScanCard.Features.Cards
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Card, Card>();
        }
    }
}