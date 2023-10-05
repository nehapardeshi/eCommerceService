namespace eCommerceService.Entities.Enums
{
    public enum OrderStatus
    {
        Draft = 1, // Customer created new order
        Paid, // Customer paid, order can be shipped now
        Shipped, // Order items shipped
        Delivered, // Order items received by the customer
        Cancelled // Order cancelled
    }
}
