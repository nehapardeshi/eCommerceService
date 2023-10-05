using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using eCommerceService.Exceptions;
using eCommerceService.Repositories;
using eCommerceService.Services.Validators;
using System.Transactions;

namespace eCommerceService.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IProductsService _productsService;
        private readonly IEnumerable<IOrderValidator> _orderValidators;

        public OrdersService(IOrdersRepository ordersRepository, IProductsService productsService, IEnumerable<IOrderValidator> orderValidators)
        {
            _ordersRepository = ordersRepository;
            _productsService = productsService;
            _orderValidators = orderValidators;
        }

        public async Task<Order> AddOrderAsync(int customerId, string currencyCode, string? streetAddress = null, string? postalCode = null, string? city = null, string? country = null)
        {
            var currentDate = DateTime.Now;
            var order = new Order
            {
                CustomerId = customerId,
                CreatedDate = currentDate,
                CurrencyCode = currencyCode,
                OrderNumber = currentDate.ToString("yyyyMMddHHmmssfffff"),
                OrderStatus = OrderStatus.Draft,
                StreetAddress = streetAddress,
                PostalCode = postalCode,
                City = city,
                Country = country
            };

            await _ordersRepository.AddOrderAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(int orderId, string? streetAddress = null, string? postalCode = null, string? city = null, string? country = null)
        {
            var order = await GetOrderAsync(orderId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), orderId));
            order.StreetAddress = streetAddress;
            order.PostalCode = postalCode;
            order.City = city;
            order.Country = country;
            await _ordersRepository.UpdateOrderAsync(order);

            return order;
        }

        public async Task<OrderItem> AddOrderItemAsync(int orderId, int productId, int quantity)
        {
            var order = await _ordersRepository.GetOrderFullAsync(orderId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), orderId));

            var validtor = _orderValidators.SingleOrDefault(v => v.OrderAction == OrderAction.ChangeInOrderItems) ?? throw new Exception("Ship order validator not found");
            validtor.ValidateOrder(order);

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Reserve product
            var reservedTotalAmount = await _productsService.ReserveProductAsync(productId, quantity);

            // If product reserved, then add order item to the order
            var orderItem = new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = quantity,
                Amount = reservedTotalAmount,
                CreatedDate = DateTime.Now
            };

            await _ordersRepository.AddOrderItemAsync(orderItem);

            // Finally, update order amount
            order.TotalAmount += reservedTotalAmount;
            await _ordersRepository.UpdateOrderAsync(order);

            transactionScope.Complete();
            return orderItem;
        }

        public async Task RemoveOrderItemAsync(int orderId, int orderItemId)
        {
            var order = await GetOrderAsync(orderId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), orderId));

            var validtor = _orderValidators.SingleOrDefault(v => v.OrderAction == OrderAction.ChangeInOrderItems) ?? throw new Exception("Ship order validator not found");
            validtor.ValidateOrder(order);

            var orderItem = await _ordersRepository.GetOrderItemAsync(orderItemId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(OrderItem), orderItemId));
            
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Unreserve product
            var unreservedTotalAmount = await _productsService.UnreserveProductAsync(orderItem.ProductId, orderItem.Quantity);

            // Remove Order Item
            await _ordersRepository.DeleteOrderItemAsync(orderItem);

            // Finally, update order amount
            order.TotalAmount -= unreservedTotalAmount;
            await _ordersRepository.UpdateOrderAsync(order);

            transactionScope.Complete();
        }

        public async Task UpdateOrderItemAsync(int orderId, int orderItemId, int quantityToUpdate)
        {
            // If quantityToUpdate is 0, then remove order item
            if (quantityToUpdate == 0)
            {
                await RemoveOrderItemAsync(orderId, orderItemId);
                return;
            }

            // Else if quantityToUpdate == order item quanity, then do nothing
            var order = await GetOrderAsync(orderId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), orderId));
            var orderItem = await _ordersRepository.GetOrderItemAsync(orderItemId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(OrderItem), orderItemId));

            if (orderItem.Quantity == quantityToUpdate)
            {
                // No need to update
                return;
            }

            var validtor = _orderValidators.SingleOrDefault(v => v.OrderAction == OrderAction.ChangeInOrderItems) ?? throw new Exception("Ship order validator not found");
            validtor.ValidateOrder(order);

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // If quantityToUpdate less than order item quantity, then unreserve order item quantity - quantityToUpdate
            if (quantityToUpdate < orderItem.Quantity)
            {
                var quantityToUnreserve = orderItem.Quantity - quantityToUpdate;
                var unreservedTotalAmount = await _productsService.UnreserveProductAsync(orderItem.ProductId, quantityToUnreserve);
                orderItem.Amount -= unreservedTotalAmount;
                orderItem.Quantity = quantityToUpdate;
                order.TotalAmount -= unreservedTotalAmount;
            }
            else
            {
                // If quantityToUpdate greater than order item quantity, then reserve quantityToUpdate - order item quantity
                var quantityToReserve = quantityToUpdate - orderItem.Quantity;
                var reservedTotalAmount = await _productsService.ReserveProductAsync(orderItem.ProductId, quantityToReserve);
                orderItem.Quantity = quantityToUpdate;
                orderItem.Amount += reservedTotalAmount;
                order.TotalAmount += reservedTotalAmount;
            }

            // Finally, update order item and order 
            await _ordersRepository.UpdateOrderItemAsync(orderItem);
            await _ordersRepository.UpdateOrderAsync(order);

            transactionScope.Complete();
        }

        public async Task PayOrderAsync(int orderId)
        {
            var order = await GetOrderAsync(orderId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), orderId));

            var validtor = _orderValidators.SingleOrDefault(v => v.OrderAction == OrderAction.Pay) ?? throw new Exception("Pay order validator not found");
            validtor.ValidateOrder(order);

            // Update order status to Paid and set payment date
            order.OrderStatus = OrderStatus.Paid;
            order.PaymentDate = DateTime.Now;
            await _ordersRepository.UpdateOrderAsync(order);
        }

        public async Task ShipOrderAsync(int orderId)
        {
            var order = await _ordersRepository.GetOrderFullAsync(orderId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), orderId));

            var validtor = _orderValidators.SingleOrDefault(v => v.OrderAction == OrderAction.Ship) ?? throw new Exception("Ship order validator not found");
            validtor.ValidateOrder(order);

            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Update OnHand, Reserved and Available product quantity for each order item
            foreach (var item in order.OrderItems)
            {
                await _productsService.ShipProductAsync(item.ProductId, item.Quantity);
            }

            // Update order status to Shipped and shipped date
            order.OrderStatus = OrderStatus.Shipped;
            order.ShippedDate = DateTime.Now;
            await _ordersRepository.UpdateOrderAsync(order);

            transactionScope.Complete();
        }

        public async Task DeliverOrderAsync(int orderId)
        {
            var order = await GetOrderAsync(orderId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), orderId));

            var validtor = _orderValidators.SingleOrDefault(v => v.OrderAction == OrderAction.Deliver) ?? throw new Exception("Pay order validator not found");
            validtor.ValidateOrder(order);

            // Update order status to delivered and set DeliveredDate
            order.OrderStatus = OrderStatus.Delivered;
            order.DeliveredDate = DateTime.Now;
            await _ordersRepository.UpdateOrderAsync(order);
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await GetOrderAsync(orderId) ?? throw new ECommerceException(ErrorCode.NotFound(typeof(Order), orderId));

            var validtor = _orderValidators.SingleOrDefault(v => v.OrderAction == OrderAction.Cancel) ?? throw new Exception("Pay order validator not found");
            validtor.ValidateOrder(order);

            // Update order status to cancelled and set CancelledDate
            order.OrderStatus = OrderStatus.Cancelled;
            order.CancelledDate = DateTime.Now;
            await _ordersRepository.UpdateOrderAsync(order);
        }

        public async Task<Order> GetOrderAsync(int id) => await _ordersRepository.GetOrderAsync(id);
        public async Task<Order> GetOrderFullAsync(int id) => await _ordersRepository.GetOrderFullAsync(id);
    }
}
