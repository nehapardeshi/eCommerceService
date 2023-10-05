using eCommerceService.Database;
using eCommerceService.Entities;
using eCommerceService.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace eCommerceService.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ECommerceDbContext _dbContext;
        public ProductsRepository(ECommerceDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddProductAsync(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductAsync(int id)
            => await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        public async Task<IEnumerable<Product>> GetProductsAsync(string searchText = "")
        {
            if (string.IsNullOrEmpty(searchText))
                return await _dbContext.Products.ToListAsync();

            searchText = searchText.ToLower();
            return _dbContext.Products
                .Where(p => p.Name.ToLower().Contains(searchText) ||
                p.Description.ToLower().Contains(searchText) || 
                p.SKU.ToLower().Contains(searchText)).ToList();
        }

        public async Task UpdateProductAsync(Product product)
        {
            //First check ProductId:
            var prod = await GetProductAsync(product.Id) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Product), product.Id));

            // Updates the product properties:
            prod.Description = product.Description;
            prod.UnitPrice = product.UnitPrice;
            prod.SKU = product.SKU;
            prod.ReservedQuantity = product.ReservedQuantity;
            prod.UpdatedDate = product.UpdatedDate;
            prod.AvailableQuantity = product.AvailableQuantity;
            prod.IsActive = product.IsActive;
            prod.Name = product.Name;

            await _dbContext.SaveChangesAsync();
        }
    }
}
