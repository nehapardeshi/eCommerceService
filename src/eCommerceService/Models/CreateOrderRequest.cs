namespace eCommerceService.Models
{
    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public string CurrencyCode { get; set; }
        public List<CreateOrderItemRequest> OrderItems { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}