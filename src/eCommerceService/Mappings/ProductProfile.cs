using AutoMapper;

namespace eCommerceService.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Entities.Product, Models.Product>();
            CreateMap<Entities.Enums.ProductCategory, Models.ProductCategory>();
            CreateMap<Models.ProductCategory, Entities.Enums.ProductCategory>();
        }
    }
}
