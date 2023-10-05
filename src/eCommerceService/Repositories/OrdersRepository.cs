using eCommerceService.Database;
using eCommerceService.Entities;
using eCommerceService.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace eCommerceService.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly ECommerceDbContext _dbContext;
        public OrdersRepository(ECommerceDbContext context)
        {
            _dbContext = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
        }   

        public async Task UpdateOrderAsync(Order order)
        {
            var ord = await GetOrderAsync(order.Id) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), order.Id));

            ord.TotalAmount = order.TotalAmount;
            ord.OrderStatus = order.OrderStatus;
            ord.CancelledDate = order.CancelledDate;
            ord.DeliveredDate = order.DeliveredDate;
            ord.PaymentDate = order.PaymentDate;
            ord.ShippedDate = order.ShippedDate;
            ord.StreetAddress = order.StreetAddress;
            ord.City = order.City;
            ord.PostalCode = order.PostalCode;
            ord.Country = order.Country;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateOrderItemAsync(OrderItem orderItem)
        {
            var ordItem = await GetOrderItemAsync(orderItem.Id) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(OrderItem), orderItem.Id));

            ordItem.Quantity = orderItem.Quantity;
            ordItem.UpdatedDate = DateTime.Now;
            ordItem.Amount = orderItem.Amount;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Order> GetOrderAsync(int id)
            => await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Order> GetOrderFullAsync(int id)
            => await _dbContext.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddOrderItemAsync(OrderItem orderItem)
            => await _dbContext.OrderItems.AddAsync(orderItem);

        public async Task DeleteOrderItemAsync(OrderItem orderItem)
        {
            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<OrderItem> GetOrderItemAsync(int id)
        => await _dbContext.OrderItems.FirstOrDefaultAsync(o => o.Id == id);
    }
}
