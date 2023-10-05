using eCommerceService.Entities;
using eCommerceService.Exceptions;
using eCommerceService.Repositories;
using eCommerceService.Services;
using NSubstitute;

namespace eCommerceService.UnitTests
{
    public class ProductsServiceTest
    {
        private readonly IProductsService _productsService;
        private readonly IProductsRepository _productsRepository;

        public ProductsServiceTest()
        {
            _productsRepository = Substitute.For<IProductsRepository>();
            _productsService = new ProductsService(_productsRepository);
        }

        [Fact]
        public async Task ReserveProductTestAsync()
        {
            // Arrange
            var productId = 1;
            var quantityToReserve = 2;
            var expectedAvailableQuantity = 8;
            var expectedReservedQuantity = 2;
            var expectedReservedTotalAmount = 1000;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 500,
                AvailableQuantity = 10,
                ReservedQuantity = 0
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            // Act
            var actualReservedTotalAmount = await _productsService.ReserveProductAsync(productId, quantityToReserve);

            // Assert
            Assert.Equal(expectedAvailableQuantity, product.AvailableQuantity);
            Assert.Equal(expectedReservedQuantity, product.ReservedQuantity);
            Assert.Equal(expectedReservedTotalAmount, actualReservedTotalAmount);
        }

        [Fact]
        public async Task ReserveProductTest_ProductQuantityNotAvailableAsync()
        {
            // Arrange
            var productId = 1;
            var quantityToReserve = 12;
            var expectedErrorCode = ErrorCode.ProductQuantityNotAvailable;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 500,
                AvailableQuantity = 10,
                ReservedQuantity = 0
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            // Act
            try
            {
                var actualReservedTotalAmount = await _productsService.ReserveProductAsync(productId, quantityToReserve);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.True(ex is ECommerceException);
                Assert.Equal(expectedErrorCode.ErrorCodeName, (ex as ECommerceException).ErrorCode.ErrorCodeName);
            }
        }

        [Fact]
        public async Task UnreserveProductTestAsync()
        {
            // Arrange
            var productId = 1;
            var expectedAvailableQuantity = 10;
            var expectedReservedQuantity = 0;
            var expectedUnreservedTotalAmount = 1000;
            var quantityToReserve = 2;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 500,
                AvailableQuantity = 8,
                ReservedQuantity = 2
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            // Act
            var actualUnreservedTotalAmount = await _productsService.UnreserveProductAsync(productId, quantityToReserve);

            // Assert
            Assert.Equal(expectedAvailableQuantity, product.AvailableQuantity);
            Assert.Equal(expectedReservedQuantity, product.ReservedQuantity);
            Assert.Equal(expectedUnreservedTotalAmount, actualUnreservedTotalAmount);
        }

        [Fact]
        public async Task ShipProductTestAsync()
        {
            // Arrange
            var productId = 1;
            var quantityToReserve = 2;
            var expectedAvailableQuantity = 8;
            var expectedReservedQuantity = 0;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 500,
                AvailableQuantity = 8,
                ReservedQuantity = 2
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            // Act
            await _productsService.ShipProductAsync(productId, quantityToReserve);

            // Assert
            Assert.Equal(expectedAvailableQuantity, product.AvailableQuantity);
            Assert.Equal(expectedReservedQuantity, product.ReservedQuantity);
        }
    }
}