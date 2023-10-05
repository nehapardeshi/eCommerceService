using eCommerceService.Entities;
using eCommerceService.Entities.Enums;

namespace eCommerceService.Models
{
    public class Order
    {
        public int Id { get; set; } //PK
        public string OrderNumber { get; set; }
        public int CustomerId { get; set; }//FK
        public string CurrencyCode { get; set; }
        public decimal TotalAmount { get; set; } = 0;
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CancelledDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
