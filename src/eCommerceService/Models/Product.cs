namespace eCommerceService.Models
{
    public enum ProductCategory
    {
        Men = 1,
        Women = 2,
        Kids = 3,
        Others = 4
    }

    public class Product
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public int OnHandQuantity { get; set; }
        public int ReservedQuantity { get; set; } //"Reserved" refers to items that have been ordered, but are still physically in your warehouse. Items are reserved (or set aside) until the order is either Completed (shipped out) or Canceled.
        public int AvailableQuantity { get; set; } //"Available" refers to the quantity that is currently available for customers to purchase(total on-hand minus reserved).
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public decimal UnitPrice { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
