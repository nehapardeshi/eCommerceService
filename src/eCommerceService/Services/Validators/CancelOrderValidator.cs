using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;

namespace eCommerceService.Services.Validators
{
    public class CancelOrderValidator : OrderValidator
    {
        public override OrderAction OrderAction => OrderAction.Cancel;
        public override bool Validate(Order order)
        {
            if (order.OrderStatus != OrderStatus.Draft)
            {
                throw new ECommerceException(ErrorCode.OrderAlreadyPaid, $"Cannot cancel paid order id {order.Id}");
            }

            return true;
        }
    }
}
