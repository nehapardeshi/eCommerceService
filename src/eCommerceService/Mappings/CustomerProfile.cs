using AutoMapper;

namespace eCommerceService.Mappings
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Entities.Customer, Models.Customer>();
        }
    }
}
