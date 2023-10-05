using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;

namespace eCommerceService.Services.Validators
{
    public class PayOrderValidator : OrderValidator
    {
        public override OrderAction OrderAction => OrderAction.Pay;

        public override bool Validate(Order order)
        {
            if (order.TotalAmount == 0)
            {
                throw new ECommerceException(ErrorCode.NoOrderItemAvailableToPay, $"No order items exists in the order id {order.Id} to make payment");
            }

            if (order.OrderStatus != OrderStatus.Draft)
            {
                throw new ECommerceException(ErrorCode.OrderAlreadyPaid, $"Cannot make payment to already paid order id {order.Id}");
            }

            if (string.IsNullOrEmpty(order.StreetAddress) ||  string.IsNullOrEmpty(order.City) || string.IsNullOrEmpty(order.Country) || string.IsNullOrEmpty(order.PostalCode))
            {
                throw new ECommerceException(ErrorCode.ShippingAddressMissing, $"Shipping address missing for the order id {order.Id}");
            }

            return true;
        }
    }
}