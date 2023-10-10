using AutoMapper;
using eCommerceService.Models;
using eCommerceService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace eCommerceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _orderService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public OrdersController(IOrdersService orderService, IMapper mapper, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _mapper = mapper;
            _logger = logger;
        }


        /// <summary>
        /// Creates new order
        /// </summary>
        /// <param name="request">Create order request</param>
        /// <returns>Created order</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.Order), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddOrderAsync([FromBody] CreateOrderRequest request)
        {
            var createdOrder = await _orderService.AddOrderAsync(request.CustomerId, request.CurrencyCode, request.StreetAddress, request.PostalCode, request.City, request.Country);
            var orderItems = new List<Entities.OrderItem>();
            if (request.OrderItems.Any())
            {
                foreach (var item in request.OrderItems)
                {
                    var orderItem = await _orderService.AddOrderItemAsync(createdOrder.Id, item.ProductId, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }

            // Fetch order with refreshed total amount values
            var orderResponse = _mapper.Map<Entities.Order, Models.Order>(await _orderService.GetOrderAsync(createdOrder.Id));

            _logger.LogInformation($"Order created with Id: {createdOrder.Id}");
            return Ok(orderResponse);
        }

        /// <summary>
        /// Gets order details with the given order id
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{order-id}")]
        [ProducesResponseType(typeof(Models.Order), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetOrderAsync([FromRoute(Name = "order-id")] int orderId)
        {
            var orderResponse = _mapper.Map<Entities.Order, Models.Order>(await _orderService.GetOrderFullAsync(orderId));
            return Ok(orderResponse);
        }

        /// <summary>
        /// Updates an order
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{order-id}")]
        [ProducesResponseType(typeof(Models.Order), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateOrderAsync([FromRoute(Name = "order-id")] int orderId, [FromBody] UpdateOrderRequest request)
        {
            var updatedOrder = await _orderService.UpdateOrderAsync(orderId, request.StreetAddress, request.PostalCode, request.City, request.Country);
            var orderResponse = _mapper.Map<Entities.Order, Models.Order>(updatedOrder);
            return Ok(orderResponse);
        }

        /// <summary>
        /// Adds an order item to an proder
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="request">CreateOrderItemRequest</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{order-id}/order-items")]
        [ProducesResponseType(typeof(Models.OrderItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateOrderItemAsync([FromRoute(Name = "order-id")] int orderId, [FromBody] CreateOrderItemRequest request)
        {
            var orderItem = await _orderService.AddOrderItemAsync(orderId, request.ProductId, request.Quantity);
            var orderItemResponse = _mapper.Map<Entities.OrderItem, Models.OrderItem>(orderItem);

            _logger.LogInformation($"Order item created for order id: {orderId} with order item id: {orderItem.Id}");
            return Ok(orderItemResponse);
        }

        /// <summary>
        /// Updates an order item, can only update quantity of the product
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="orderItemId">Order Item Id</param>
        /// <param name="quantityToUpdate">Quantity to update</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{order-id}/order-items/{order-item-id}")]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateOrderItemAsync([FromRoute(Name = "order-id")] int orderId, [FromRoute(Name = "order-item-id")] int orderItemId, [FromQuery(Name = "quantity-to-update")] int quantityToUpdate)
        {
            await _orderService.UpdateOrderItemAsync(orderId, orderItemId, quantityToUpdate);
            _logger.LogInformation($"Order item updated with Id: {orderItemId}");

            return Ok();
        }

        /// <summary>
        /// Marks an order as paid 
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{order-id}/pay")]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PayOrderAsync([FromRoute(Name = "order-id")] int orderId)
        {
            await _orderService.PayOrderAsync(orderId);
            _logger.LogInformation($"Order paid with Id: {orderId}");
            return Ok();
        }

        /// <summary>
        /// Ships an order
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{order-id}/ship")]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ShipOrderAsync([FromRoute(Name = "order-id")] int orderId)
        {
            await _orderService.ShipOrderAsync(orderId);
            _logger.LogInformation($"Order shipped with Id: {orderId}");
            return Ok();
        }

        /// <summary>
        /// Marks an order as delivered
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{order-id}/deliver")]
        public async Task<IActionResult> DeliverOrderAsync([FromRoute(Name = "order-id")] int orderId)
        {
            await _orderService.DeliverOrderAsync(orderId);
            _logger.LogInformation($"Order delivered with Id: {orderId}");
            return Ok();
        }

        /// <summary>
        /// Cancels an order
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{order-id}/cancel")]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CancelOrderAsync([FromRoute(Name = "order-id")] int orderId)
        {
            await _orderService.CancelOrderAsync(orderId);
            _logger.LogInformation($"Order cancelled with Id: {orderId}");
            return Ok();
        }

        /// <summary>
        /// Removes an order item from an order
        /// </summary>
        /// <param name="orderId">Order Id</param>
        /// <param name="orderItemId">Order Item Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{order-id}/order-items/{order-item-id}")]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveOrderItemAsync([FromRoute(Name = "order-id")] int orderId, [FromRoute(Name = "order-item-id")] int orderItemId)
        {
            await _orderService.RemoveOrderItemAsync(orderId, orderItemId);
            _logger.LogInformation($"Order item removed with Id: {orderItemId}");
            return Ok();
        }
    }
}