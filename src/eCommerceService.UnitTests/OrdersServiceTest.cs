using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;
using eCommerceService.Repositories;
using eCommerceService.Services;
using eCommerceService.Services.Validators;
using NSubstitute;

namespace eCommerceService.UnitTests
{
    public class OrdersServiceTest
    {
        private readonly IOrdersService _ordersService;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductsService _productsService;
        private readonly IProductsRepository _productsRepository;

        public OrdersServiceTest()
        {
            _productsRepository = Substitute.For<IProductsRepository>();
            _ordersRepository = Substitute.For<IOrdersRepository>();
            _productsService = new ProductsService(_productsRepository);
            _ordersService = new OrdersService(_ordersRepository, _productsService, GetOrderValidators());
        }

        private static IEnumerable<IOrderValidator> GetOrderValidators()
        {
            var validators = new List<OrderValidator>
            {
                new PayOrderValidator(),
                new ShipOrderValidator(),
                new CancelOrderValidator(),
                new DeliverOrderValidator(),
                new ChangeInOrderItemsValidator()
            };
            return validators;
        }

        [Fact]
        public async Task AddOrderTestAsync()
        {
            // Arrange
            var customerId = 1;
            var currencyCode = "NOK";
            var expectedOrderStatus = OrderStatus.Draft;
            var expectedOrderItemsCount = 0;
            var expectedTotalAmount = 0;

            // Act
            var order = await _ordersService.AddOrderAsync(customerId, currencyCode);

            // Assert
            Assert.NotNull(order);
            Assert.Equal(expectedOrderItemsCount, order.OrderItems.Count);
            Assert.Equal(expectedOrderStatus, order.OrderStatus);
            Assert.Equal(customerId, order.CustomerId);
            Assert.Equal(currencyCode, order.CurrencyCode);
            Assert.Equal(expectedTotalAmount, order.TotalAmount);
        }

        [Fact]
        public async Task AddOrderItemTestAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var customerId = 1;
            var expectedProductId = 1;
            var expectedQuantity = 2;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderItemAmount = 1000;
            var expectedOrderTotalAmount = 1000;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 500,
                AvailableQuantity = 10,
                ReservedQuantity = 0
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus
            };

            _ordersRepository.GetOrderFullAsync(orderId).Returns(order);

            // Act
            var orderItem = await _ordersService.AddOrderItemAsync(orderId, expectedProductId, expectedQuantity);

            // Assert
            Assert.Equal(expectedProductId, orderItem.ProductId);
            Assert.Equal(expectedQuantity, orderItem.Quantity);
            Assert.Equal(expectedOrderItemAmount, orderItem.Amount);
            Assert.Equal(expectedOrderTotalAmount, order.TotalAmount);
        }

        [Fact]
        public async Task AddSecondOrderItemTestAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var customerId = 1;
            var expectedProductId = 1;
            var expectedQuantity = 2;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderItemAmount = 1000;
            var expectedOrderTotalAmount = 1200;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 500,
                AvailableQuantity = 10,
                ReservedQuantity = 0
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = 1, OrderId = orderId, Amount = 200, ProductId = productId }
                },
                TotalAmount = 200
            };

            _ordersRepository.GetOrderFullAsync(orderId).Returns(order);

            // Act
            var orderItem = await _ordersService.AddOrderItemAsync(orderId, expectedProductId, expectedQuantity);

            // Assert
            Assert.Equal(expectedProductId, orderItem.ProductId);
            Assert.Equal(expectedQuantity, orderItem.Quantity);
            Assert.Equal(expectedOrderItemAmount, orderItem.Amount);
            Assert.Equal(expectedOrderTotalAmount, order.TotalAmount);
        }

        [Fact]
        public async Task RemoveOrderItemTestAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderTotalAmount = 0;
            var orderItemIdToRemove = 1;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 500,
                AvailableQuantity = 10,
                ReservedQuantity = 0
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = 1, OrderId = orderId, Amount = 1000, ProductId = productId,Quantity = 2 }
                },
                TotalAmount = 1000
            };

            _ordersRepository.GetOrderAsync(orderId).Returns(order);

            _ordersRepository.GetOrderItemAsync(orderItemIdToRemove).Returns(order.OrderItems.First(o => o.Id == orderItemIdToRemove));

            // Act
            await _ordersService.RemoveOrderItemAsync(orderId, orderItemIdToRemove);

            // Assert
            Assert.Equal(expectedOrderTotalAmount, order.TotalAmount);

            
        }

        [Fact]
        public async Task RemoveSecondOrderItemTestAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 2;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderTotalAmount = 200;
            var orderItemIdToRemove = 2;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 500,
                AvailableQuantity = 10,
                ReservedQuantity = 0
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = 1, OrderId = orderId, Amount = 200, ProductId = 1, Quantity = 2 },
                    new OrderItem { Id = 2, OrderId = orderId, Amount = 1000, ProductId = productId, Quantity = 2 }
                },
                TotalAmount = 1200
            };

            _ordersRepository.GetOrderAsync(orderId).Returns(order);
            _ordersRepository.GetOrderItemAsync(orderItemIdToRemove).Returns(order.OrderItems.First(o => o.Id == orderItemIdToRemove));

            // Act
            await _ordersService.RemoveOrderItemAsync(orderId, orderItemIdToRemove);

            // Assert
            Assert.Equal(expectedOrderTotalAmount, order.TotalAmount);
        }

        [Fact]
        public async Task UpdateOrderItemTest_ReduceQuantityAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderTotalAmount = 200;
            var orderItemIdToUpdate = 1;
            var expectedUpdatedQuantity = 1;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 200,
                AvailableQuantity = 8,
                ReservedQuantity = 2
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = orderItemIdToUpdate, OrderId = orderId, Amount = 400, ProductId = productId, Quantity = 2 }
                },
                TotalAmount = 400
            };

            _ordersRepository.GetOrderAsync(orderId).Returns(order);
            _ordersRepository.GetOrderItemAsync(orderItemIdToUpdate).Returns(order.OrderItems.First(o => o.Id == orderItemIdToUpdate));

            // Act
            await _ordersService.UpdateOrderItemAsync(orderId, orderItemIdToUpdate, expectedUpdatedQuantity);

            // Assert
            var orderItem = await _ordersRepository.GetOrderItemAsync(orderItemIdToUpdate);
            Assert.NotNull(orderItem);
            Assert.Equal(expectedOrderTotalAmount, order.TotalAmount);
            Assert.Equal(expectedOrderTotalAmount, orderItem.Amount);
            Assert.Equal(expectedUpdatedQuantity, orderItem.Quantity);
        }

        [Fact]
        public async Task UpdateOrderItemTest_IncreaseQuantityAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderTotalAmount = 800;
            var orderItemIdToUpdate = 1;
            var expectedUpdatedQuantity = 4;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 200,
                AvailableQuantity = 8,
                ReservedQuantity = 2
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = orderItemIdToUpdate, OrderId = orderId, Amount = 400, ProductId = productId, Quantity = 2 }
                },
                TotalAmount = 400
            };

            _ordersRepository.GetOrderAsync(orderId).Returns(order);
            _ordersRepository.GetOrderItemAsync(orderItemIdToUpdate).Returns(order.OrderItems.First(o => o.Id == orderItemIdToUpdate));

            // Act
            await _ordersService.UpdateOrderItemAsync(orderId, orderItemIdToUpdate, expectedUpdatedQuantity);

            // Assert
            var orderItem = await _ordersRepository.GetOrderItemAsync(orderItemIdToUpdate);
            Assert.NotNull(orderItem);
            Assert.Equal(expectedOrderTotalAmount, order.TotalAmount);
            Assert.Equal(expectedOrderTotalAmount, orderItem.Amount);
            Assert.Equal(expectedUpdatedQuantity, orderItem.Quantity);
        }

        [Fact]
        public async Task UpdateOrderItemTest_SetQuantityToZeroAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderTotalAmount = 0;
            var orderItemIdToUpdate = 1;
            var expectedUpdatedQuantity = 0;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 200,
                AvailableQuantity = 8,
                ReservedQuantity = 2
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = orderItemIdToUpdate, OrderId = orderId, Amount = 400, ProductId = productId, Quantity = 2 }
                },
                TotalAmount = 400
            };

            _ordersRepository.GetOrderAsync(orderId).Returns(order);
            _ordersRepository.GetOrderItemAsync(orderItemIdToUpdate).Returns(order.OrderItems.First(o => o.Id == orderItemIdToUpdate));

            // Act
            await _ordersService.UpdateOrderItemAsync(orderId, orderItemIdToUpdate, expectedUpdatedQuantity);

            // Assert
            var orderItem = await _ordersRepository.GetOrderItemAsync(orderItemIdToUpdate);
            Assert.NotNull(orderItem);
            Assert.Equal(expectedOrderTotalAmount, order.TotalAmount);
        }

        [Fact]
        public async Task PayOrderTestAsync()
        {
            // Arrange
            var orderId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderStatus = OrderStatus.Paid;

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                TotalAmount = 400,
                StreetAddress = "SomeStreetAddress",
                PostalCode = "12345",
                City = "Oslo",
                Country = "Norway"
            };

            _ordersRepository.GetOrderAsync(orderId).Returns(order);

            // Act
            await _ordersService.PayOrderAsync(orderId);

            // Assert
            Assert.Equal(expectedOrderStatus, order.OrderStatus);
        }

        [Fact]
        public async Task ShipOrderTestAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var orderItemId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Paid;
            var expectedOrderStatus = OrderStatus.Shipped;
            var expectedReservedQuantity = 0;
            var expectedAvailableQuantity = 8;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 200,
                AvailableQuantity = 8,
                ReservedQuantity = 2
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = orderItemId, OrderId = orderId, Amount = 400, ProductId = productId, Quantity = 2 }
                },
                TotalAmount = 400
            };

            _ordersRepository.GetOrderFullAsync(orderId).Returns(order);

            // Act
            await _ordersService.ShipOrderAsync(orderId);

            // Assert
            Assert.Equal(expectedOrderStatus, order.OrderStatus);
            Assert.Equal(expectedReservedQuantity, product.ReservedQuantity);
            Assert.Equal(expectedAvailableQuantity, product.AvailableQuantity);
        }

        [Fact]
        public async Task ShipOrderTest_With2OrderItemsAsync()
        {
            // Arrange
            var orderId = 1;
            var productId1 = 1;
            var productId2 = 2;
            var orderItemId1 = 1;
            var orderItemId2 = 2;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Paid;
            var expectedOrderStatus = OrderStatus.Shipped;
            var expectedReservedQuantity1 = 0;
            var expectedAvailableQuantity1 = 8;
            var expectedReservedQuantity2 = 0;
            var expectedAvailableQuantity2 = 40;

            var product1 = new Product
            {
                Id = productId1,
                UnitPrice = 200,
                AvailableQuantity = 8,
                ReservedQuantity = 2
            };

            _productsRepository.GetProductAsync(productId1).Returns(product1);

            var product2 = new Product
            {
                Id = productId1,
                UnitPrice = 100,
                AvailableQuantity = 40,
                ReservedQuantity = 10
            };

            _productsRepository.GetProductAsync(productId2).Returns(product2);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = orderItemId1, OrderId = orderId, Amount = 400, ProductId = productId1, Quantity = 2 },
                    new OrderItem { Id = orderItemId2, OrderId = orderId, Amount = 1000, ProductId = productId2, Quantity = 10 }
                },
                TotalAmount = 1400
            };

            _ordersRepository.GetOrderFullAsync(orderId).Returns(order);

            // Act
            await _ordersService.ShipOrderAsync(orderId);

            // Assert
            Assert.Equal(expectedOrderStatus, order.OrderStatus);
            Assert.Equal(expectedReservedQuantity1, product1.ReservedQuantity);
            Assert.Equal(expectedAvailableQuantity1, product1.AvailableQuantity);
            Assert.Equal(expectedReservedQuantity2, product2.ReservedQuantity);
            Assert.Equal(expectedAvailableQuantity2, product2.AvailableQuantity);
        }

        [Fact]
        public async Task ShipOrderTest_WithNoOrderItemsAsync()
        {
            // Arrange
            var orderId = 1;
            var productId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedErrorCode = ErrorCode.NoOrderItemAvailableToShip;

            var product = new Product
            {
                Id = productId,
                UnitPrice = 200,
                AvailableQuantity = 8,
                ReservedQuantity = 2
            };

            _productsRepository.GetProductAsync(productId).Returns(product);

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus
            };

            _ordersRepository.GetOrderFullAsync(orderId).Returns(order);

            try
            {
                // Act
                await _ordersService.ShipOrderAsync(orderId);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.True(ex is ECommerceException);
                Assert.Equal(expectedErrorCode.ErrorCodeName, (ex as ECommerceException).ErrorCode.ErrorCodeName);
            }
        }

        [Fact]
        public async Task DeliverOrderTestAsync()
        {
            // Arrange
            var orderId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Shipped;
            var expectedOrderStatus = OrderStatus.Delivered;

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                TotalAmount = 400
            };

            _ordersRepository.GetOrderAsync(orderId).Returns(order);

            // Act
            await _ordersService.DeliverOrderAsync(orderId);

            // Assert
            Assert.Equal(expectedOrderStatus, order.OrderStatus);
        }

        [Fact]
        public async Task CancelOrderTestAsync()
        {
            // Arrange
            var orderId = 1;
            var customerId = 1;
            var currencyCode = "NOK";
            var orderStatus = OrderStatus.Draft;
            var expectedOrderStatus = OrderStatus.Cancelled;

            var order = new Order
            {
                Id = orderId,
                CreatedDate = DateTime.Now,
                CustomerId = customerId,
                CurrencyCode = currencyCode,
                OrderStatus = orderStatus,
                TotalAmount = 400
            };

            _ordersRepository.GetOrderAsync(orderId).Returns(order);

            // Act
            await _ordersService.CancelOrderAsync(orderId);

            // Assert
            Assert.Equal(expectedOrderStatus, order.OrderStatus);
        }
    }
}