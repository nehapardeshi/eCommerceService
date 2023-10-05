using eCommerceService.Entities;

namespace eCommerceService.Services
{
    public interface IOrdersService
    {
        Task<Order> GetOrderAsync(int id);
        Task<Order> GetOrderFullAsync(int id);
        Task<Order> AddOrderAsync(int customerId, string currencyCode, string? streetAddress = null, string? postalCode = null, string? city = null, string? country = null);
        Task<Order> UpdateOrderAsync(int orderId, string? streetAddress = null, string? postalCode = null, string? city = null, string? country = null);
        Task<OrderItem> AddOrderItemAsync(int orderId, int productId, int quantity);
        Task RemoveOrderItemAsync(int orderId, int orderItemId);
        Task UpdateOrderItemAsync(int orderId, int orderItemId, int quantityToUpdate);
        Task PayOrderAsync(int orderId);
        Task ShipOrderAsync(int orderId);
        Task DeliverOrderAsync(int orderId);
        Task CancelOrderAsync(int orderId);
    }
}