using eCommerceService.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eCommerceService.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; } //PK
        public string OrderNumber { get; set; }
        public int CustomerId { get; set; }//FK
        public string CurrencyCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public Customer Customer { get; set; }
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }
}