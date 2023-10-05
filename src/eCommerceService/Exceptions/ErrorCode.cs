using System.Net;

namespace eCommerceService.Exceptions
{
    public class ErrorCode
    {
        public string ErrorCodeName { get; set; }

        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public ErrorCode(string errorCodeName, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            ErrorCodeName = errorCodeName;
            Message = message;
            StatusCode = statusCode;
        }

        public ErrorCode(string errorCodeName, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : this(errorCodeName, errorCodeName, statusCode)
        {
        }

        public static ErrorCode NotFound(Type type, int id) => new($"{type.Name}NotFound", $"{type.Name} id: {id} not found");

        public static ErrorCode NoOrderItemAvailableToShip => new(nameof(NoOrderItemAvailableToShip));

        public static ErrorCode OrderNotPaid => new(nameof(OrderNotPaid));

        public static ErrorCode OrderNotShipped => new(nameof(OrderNotShipped));

        public static ErrorCode OrderAlreadyCancelled => new(nameof(OrderAlreadyCancelled));

        public static ErrorCode OrderAlreadyPaid => new(nameof(OrderAlreadyPaid));

        public static ErrorCode ShippingAddressMissing => new(nameof(ShippingAddressMissing));

        public static ErrorCode NoOrderItemAvailableToPay => new(nameof(NoOrderItemAvailableToPay));

        public static ErrorCode OrderAlreadyShipped => new(nameof(OrderAlreadyShipped));

        public static ErrorCode OrderAlreadyDelivered => new(nameof(OrderAlreadyDelivered));

        public static ErrorCode ProductQuantityNotAvailable => new(nameof(ProductQuantityNotAvailable));

        public new string ToString() => $"{ErrorCodeName}: StatusCode: '{StatusCode}', Message: '{Message}'.";
    }
}
