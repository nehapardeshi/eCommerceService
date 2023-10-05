using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;

namespace eCommerceService.Services.Validators
{
    public abstract class OrderValidator : IOrderValidator
    {
        public abstract OrderAction OrderAction { get; }

        public bool ValidateOrder(Order order)
        {
            if (order.OrderStatus == OrderStatus.Cancelled)
                throw new ECommerceException(ErrorCode.OrderAlreadyCancelled, $"Order already cancelled order id {order.Id}");

            return Validate(order);
        }

        public abstract bool Validate(Order order);
    }
}
