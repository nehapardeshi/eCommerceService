using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;

namespace eCommerceService.Services.Validators
{
    public class ChangeInOrderItemsValidator : OrderValidator
    {
        public override OrderAction OrderAction => OrderAction.ChangeInOrderItems;
        public override bool Validate(Order order)
        {
            if (order.OrderStatus != OrderStatus.Draft)
            {
                if (order.OrderStatus == OrderStatus.Cancelled)
                    throw new ECommerceException(ErrorCode.OrderAlreadyCancelled, $"Cannot add/update or remove order item to already cancelled order id {order.Id}");
                else
                    throw new ECommerceException(ErrorCode.OrderAlreadyPaid, $"Cannot add/update or remove order item to already paid order id {order.Id}");
            }

            return true;
        }
    }
}
