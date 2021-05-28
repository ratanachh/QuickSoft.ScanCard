using AutoMapper;

namespace QuickSoft.ScanCard.Features.Customers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.Customer, Customer>();
        }
    }
}