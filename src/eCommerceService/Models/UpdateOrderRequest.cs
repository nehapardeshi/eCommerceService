namespace eCommerceService.Models
{
    public class UpdateOrderRequest
    {
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}