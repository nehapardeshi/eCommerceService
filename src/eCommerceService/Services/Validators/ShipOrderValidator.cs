using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;

namespace eCommerceService.Services.Validators
{
    public class ShipOrderValidator : OrderValidator
    {
        public override OrderAction OrderAction => OrderAction.Ship;

        public override bool Validate(Order order)
        {
            if (order.TotalAmount == 0)
            {
                throw new ECommerceException(ErrorCode.NoOrderItemAvailableToShip, $"No order items exists in the order id {order.Id} for shipping");
            }

            if (order.OrderStatus != OrderStatus.Paid)
            {
                if (order.OrderStatus == OrderStatus.Draft)
                    throw new ECommerceException(ErrorCode.OrderNotPaid, $"Cannot ship unpaid order id {order.Id}");
                else
                    throw new ECommerceException(ErrorCode.OrderAlreadyShipped, $"Cannot ship already shipped order id {order.Id}");
            }

            return true;
        }
    }
}
