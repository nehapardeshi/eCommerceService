using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;
using eCommerceService.Repositories;

namespace eCommerceService.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _productsRepository;
        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task<Product> GetProductAsync(int productId)
            => await _productsRepository.GetProductAsync(productId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Product), productId));

        public async Task<IEnumerable<Product>> GetProductsAsync(string searchText = "") => await _productsRepository.GetProductsAsync(searchText);
        

        public async Task<Product> AddProductAsync(string sku, int availableQuantity, decimal unitPrice, ProductCategory catgory, string name, string description, int reservedQuantity = 0)
        {
            var product = new Product
            {
                SKU = sku,
                AvailableQuantity = availableQuantity,
                UnitPrice = unitPrice,
                ProductCategory = catgory,
                Name = name,    
                Description = description,
                ReservedQuantity = reservedQuantity,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            await _productsRepository.AddProductAsync(product);
            return product;
        }

        public async Task<Product> UpdateProductAsync(int productId, string sku, int availableQuantity, decimal unitPrice, ProductCategory category, string name, string description)
        {
            var product = await GetProductAsync(productId);
            product.UnitPrice = unitPrice;
            product.Name = name;    
            product.Description = description;
            product.SKU = sku;
            product.AvailableQuantity = availableQuantity;
            product.ProductCategory = category;
            product.UpdatedDate = DateTime.Now;

            await _productsRepository.UpdateProductAsync(product);
            return product;
        }

        public async Task<decimal> ReserveProductAsync(int productId, int quantityToReserve)
        {
            var product = await GetProductAsync(productId);
            if (product.AvailableQuantity < quantityToReserve)
            {
                throw new ECommerceException(ErrorCode.ProductQuantityNotAvailable, $"Requested product quantity not available. Requested quantity to reserve: {quantityToReserve}, Available quantity: {product.AvailableQuantity}");
            }

            product.AvailableQuantity -= quantityToReserve;
            product.ReservedQuantity += quantityToReserve;

            await _productsRepository.UpdateProductAsync(product);

            decimal reservedTotalAmount = (decimal)quantityToReserve * product.UnitPrice;
            return reservedTotalAmount;
        }

        public async Task<decimal> UnreserveProductAsync(int productId, int quantityToUnreserve)
        {
            var product = await GetProductAsync(productId);
            product.AvailableQuantity += quantityToUnreserve;
            DecreaseReservedQuantity(quantityToUnreserve, product);

            await _productsRepository.UpdateProductAsync(product);

            decimal unreservedTotalAmount = (decimal)quantityToUnreserve * product.UnitPrice;
            return unreservedTotalAmount;
        }

        public async Task ShipProductAsync(int productId, int quantityToShip)
        {
            var product = await GetProductAsync(productId);
            DecreaseReservedQuantity(quantityToShip, product);

            await _productsRepository.UpdateProductAsync(product);
        }

        public async Task InActiveProductAsync(int productId)
        {
            var product = await GetProductAsync(productId);
            product.IsActive = false;
            await _productsRepository.UpdateProductAsync(product);
        }


        private static void DecreaseReservedQuantity(int quantityToDecrease, Product product)
        {
            product.ReservedQuantity -= quantityToDecrease;

            if (product.ReservedQuantity < 0)
                product.ReservedQuantity = 0;
        }
    }
}