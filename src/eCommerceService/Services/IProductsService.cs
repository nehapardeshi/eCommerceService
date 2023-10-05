using eCommerceService.Entities;
using eCommerceService.Entities.Enums;

namespace eCommerceService.Services
{
    public interface IProductsService
    {
        Task<Product> AddProductAsync(string sku, int availableQuantity, decimal unitPrice, ProductCategory catgory, string name, string description, int reservedQuantity = 0);
        Task<Product> UpdateProductAsync(int productId, string sku, int availableQuantity, decimal unitPrice, ProductCategory category, string name, string description);
        Task<Product> GetProductAsync(int productId);
        Task<IEnumerable<Product>> GetProductsAsync(string searchText = "");
        Task<decimal> ReserveProductAsync(int productId, int quantityToReserve);
        Task<decimal> UnreserveProductAsync(int productId, int quantityToUnreserve);
        Task ShipProductAsync(int productId, int quantityToShip);
        Task InActiveProductAsync(int productId);
    }
}
