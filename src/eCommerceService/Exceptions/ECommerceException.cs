namespace eCommerceService.Exceptions
{
    public class ECommerceException : Exception
    {
        public ErrorCode ErrorCode { get; set; }

        public ECommerceException(ErrorCode errorCode)
            : this(errorCode, errorCode.Message)
        {
        }

        public ECommerceException(ErrorCode errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}