using AutoMapper;

namespace eCommerceService.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Entities.Order, Models.Order>();
            CreateMap<Entities.OrderItem, Models.OrderItem>();
        }
    }
}
