using eCommerceService.Entities;
using eCommerceService.Repositories;
using NSubstitute;

namespace eCommerceService.UnitTests
{
    internal static class UnitTestHelpers
    {
        public static IProductsRepository GetMockProductsRepository()
        {
            var repo = Substitute.For<IProductsRepository>();
            //repo.GetProductAsync(default).ReturnsForAnyArgs(new Product
            //{
            //    Id = 1,
            //    Name = "Oriental Bronze Chickend",
            //    Description = "The Football Is Good For Training And Recreational Purposes",
            //    ProductCategory = (ProductCategory)1,
            //    Active = true,
            //    CreatedDate = DateTime.Now,
            //    UnitPrice = 500,
            //    SKU = "SK001",
            //    AvailableQuantity = 10,
            //    ReservedQuantity = 0
            //});

            return repo;
        }

        public static IOrdersRepository GetMockOrdersRepository()
        {
            var repo = Substitute.For<IOrdersRepository>();
            repo.GetOrderAsync(default).ReturnsForAnyArgs(new Order
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                CustomerId = 1
            });

            
            return repo;
        }
    }
}