using eCommerceService.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceService.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; } // PK
        public string SKU { get; set; }

        public int OnHandQuantity 
        { 
            get
            {
                return ReservedQuantity + AvailableQuantity;
            }
        } //"On Hand" refers to how many of a product you physically have in your warehouse.
        public int ReservedQuantity { get; set; } //"Reserved" refers to items that have been ordered, but are still physically in your warehouse. Items are reserved (or set aside) until the order is either Completed (shipped out) or Canceled.
        public int AvailableQuantity { get; set; } //"Available" refers to the quantity that is currently available for customers to purchase(total on-hand minus reserved).
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
