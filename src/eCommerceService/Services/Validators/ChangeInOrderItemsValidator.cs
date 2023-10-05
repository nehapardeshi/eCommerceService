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
                throw new ECommerceException(ErrorCode.OrderAlreadyPaid, $"Cannot add/update or remove order item to already paid order id {order.Id}");

            return true;
        }
    }
}
