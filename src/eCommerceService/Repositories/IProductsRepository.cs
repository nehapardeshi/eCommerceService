using eCommerceService.Entities;

namespace eCommerceService.Repositories
{
    public interface IProductsRepository
    {
        Task AddProductAsync(Product product);
        Task<Product> GetProductAsync(int id);
        Task<IEnumerable<Product>> GetProductsAsync(string searchText = "");
        Task UpdateProductAsync(Product product);
    }
}
