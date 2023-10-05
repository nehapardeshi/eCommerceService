using eCommerceService.Entities;
using eCommerceService.Exceptions;
using eCommerceService.Services.Validators;

namespace eCommerceService.UnitTests
{
    public class PayOrderValidatorTest
    {
        private readonly IOrderValidator _validator;

        public PayOrderValidatorTest()
        {
            _validator = new PayOrderValidator();
        }

        [Fact]
        public async Task ValidateOrder_Success_TestAsync()
        {
            // Arrange
            var order = new Order 
            { 
                TotalAmount = 100, 
                OrderStatus = Entities.Enums.OrderStatus.Draft, 
                StreetAddress = "SomeStreetAddress",
                PostalCode = "12345",
                City = "Oslo",
                Country = "Norway"
            };
            var expected = true;

            // Act
            var actual = _validator.ValidateOrder(order);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task ValidateOrder_NoOrderItemAvailableToPay_TestAsync()
        {
            // Arrange
            var order = new Order { TotalAmount = 0, OrderStatus = Entities.Enums.OrderStatus.Draft };
            var expectedErrorCode = ErrorCode.NoOrderItemAvailableToPay;

            // Act
            try
            {
                _validator.ValidateOrder(order);
            }
            catch (Exception ex)
            {
                // Assert
                AssertException(expectedErrorCode, ex);
            }
        }

        [Fact]
        public async Task ValidateOrder_OrderAlreadyPaid_TestAsync()
        {
            // Arrange
            var order = new Order { TotalAmount = 100, OrderStatus = Entities.Enums.OrderStatus.Paid };
            var expectedErrorCode = ErrorCode.OrderAlreadyPaid;

            // Act
            try
            {
                _validator.ValidateOrder(order);
            }
            catch (Exception ex)
            {
                // Assert
                AssertException(expectedErrorCode, ex);
            }
        }

        [Fact]
        public async Task ValidateOrder_OrderAlreadyCancelled_TestAsync()
        {
            // Arrange
            var order = new Order { TotalAmount = 100, OrderStatus = Entities.Enums.OrderStatus.Cancelled };
            var expectedErrorCode = ErrorCode.OrderAlreadyCancelled;

            // Act
            try
            {
                _validator.ValidateOrder(order);
            }
            catch (Exception ex)
            {
                // Assert
                AssertException(expectedErrorCode, ex);
            }
        }

        [Fact]
        public async Task ValidateOrder_ShippingAddressMissing_TestAsync()
        {
            // Arrange
            var order = new Order { TotalAmount = 100, OrderStatus = Entities.Enums.OrderStatus.Draft };
            var expectedErrorCode = ErrorCode.ShippingAddressMissing;

            // Act
            try
            {
                _validator.ValidateOrder(order);
            }
            catch (Exception ex)
            {
                // Assert
                AssertException(expectedErrorCode, ex);
            }
        }

        private static void AssertException(ErrorCode expectedErrorCode, Exception ex)
        {
            Assert.True(ex is ECommerceException);
            Assert.Equal(expectedErrorCode.ErrorCodeName, (ex as ECommerceException).ErrorCode.ErrorCodeName);
        }
    }
}
