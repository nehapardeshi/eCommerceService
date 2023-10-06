using AutoMapper;
using eCommerceService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace eCommerceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customerService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CustomersController(ICustomersService customerService, IMapper mapper, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Gets customer by given customer id
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{customer-id}")]
        [ProducesResponseType(typeof(Models.Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCustomerAsync([FromRoute(Name = "customer-id")] int customerId)
        {
            var customer = await _customerService.GetCustomerAsync(customerId);
            var customerResponse = _mapper.Map<Entities.Customer, Models.Customer>(customer);
            return Ok(customerResponse);
        }

        /// <summary>
        /// Gets all customer or filtered by given search text
        /// </summary>
        /// <param name="searchText">Search text which is optional</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Models.Customer>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCustomersAsync([FromQuery(Name = "search-text")] string? searchText = "")
        {
            var customers = await _customerService.GetCustomersAsync(searchText);
            var customerResponse = _mapper.Map<IEnumerable<Entities.Customer>, IEnumerable<Models.Customer>>(customers);
            return Ok(customerResponse);
        }

        /// <summary>
        /// Adds a customer
        /// </summary>
        /// <param name="request">CreateCustomerRequest</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddCustomerAsync([FromBody] CreateCustomerRequest request)
        {
            var customer = await _customerService.AddCustomerAsync(request.FirstName, request.LastName, request.Email, request.Phone, request.StreetAddress, request.PostalCode, request.City, request.Country);
            var customerResponse = _mapper.Map<Entities.Customer, Models.Customer>(customer);
            _logger.LogInformation($"Customer created with Id: {customerResponse.Id}");
            return Ok(customerResponse);
        }

        /// <summary>
        /// Updates a customer
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <param name="request">UpdateCustomerRequest</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{customer-id}")]
        [ProducesResponseType(typeof(Models.Customer), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateCustomerAsync([FromRoute(Name = "customer-id")] int customerId, [FromBody] UpdateCustomerRequest request)
        {
            var customer = await _customerService.UpdateCustomerAsync(customerId, request.FirstName, request.LastName, request.Email, request.Phone, request.StreetAddress, request.PostalCode, request.City, request.Country);
            var customerResponse = _mapper.Map<Entities.Customer, Models.Customer>(customer);
            _logger.LogInformation($"Customer updated with Id: {customerResponse.Id}");
            return Ok(customerResponse);
        }

        /// <summary>
        /// Deletes a customer. It will soft delete a customer marking it as inactive
        /// </summary>
        /// <param name="customerId">Customer Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{customer-id}")]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteCustomerAsync([FromRoute(Name = "customer-id")] int customerId)
        {
            await _customerService.InActiveCustomerAsync(customerId);
            return Ok();
        }
    }
}
