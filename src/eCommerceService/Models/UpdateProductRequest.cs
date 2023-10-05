namespace eCommerceService.Models
{
    public class UpdateProductRequest
    {
        public string SKU { get; set; }
        public int AvailableQuantity { get; set; } //"Available" refers to the quantity that is currently available for customers to purchase(total on-hand minus reserved).
        public decimal UnitPrice { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
