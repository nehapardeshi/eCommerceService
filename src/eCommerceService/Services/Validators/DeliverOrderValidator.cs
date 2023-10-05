using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;

namespace eCommerceService.Services.Validators
{
    public class DeliverOrderValidator : OrderValidator
    {
        public override OrderAction OrderAction => OrderAction.Deliver;
        public override bool Validate(Order order)
        {
            if (order.OrderStatus != OrderStatus.Shipped)
            {
                if (order.OrderStatus == OrderStatus.Draft || order.OrderStatus == OrderStatus.Paid)
                    throw new ECommerceException(ErrorCode.OrderNotShipped, $"Cannot deliver unshipped order id {order.Id}");
                else
                    throw new ECommerceException(ErrorCode.OrderAlreadyDelivered, $"Cannot deliver already delivered order id {order.Id}");
            }

            return true;
        }
    }
}
