using eCommerceService.Entities;

namespace eCommerceService.Repositories
{
    public interface IOrdersRepository
    {
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task<Order> GetOrderAsync(int id);
        Task<Order> GetOrderFullAsync(int id);
        Task AddOrderItemAsync(OrderItem orderItem);
        Task<OrderItem> GetOrderItemAsync(int id);
        Task DeleteOrderItemAsync(OrderItem orderItem);
        Task UpdateOrderItemAsync(OrderItem orderItem);
    }
}
