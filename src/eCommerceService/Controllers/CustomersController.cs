using AutoMapper;
using eCommerceService.Models;
using eCommerceService.Services;
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

        [HttpGet]
        [ProducesResponseType(typeof(List<Models.Customer>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetCustomersAsync([FromQuery(Name = "search-text")] string? searchText = "")
        {
            var customers = await _customerService.GetCustomersAsync(searchText);
            var customerResponse = _mapper.Map<IEnumerable<Entities.Customer>, IEnumerable<Models.Customer>>(customers);
            return Ok(customerResponse);
        }

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
