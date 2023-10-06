using AutoMapper;
using eCommerceService.Models;
using eCommerceService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace eCommerceService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ProductsController(IProductsService productService, IMapper mapper, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get all products or filtered by search text
        /// </summary>
        /// <param name="searchText">Search Text which is optional</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Models.Product>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetProductsAsync([FromQuery(Name = "search-text")] string? searchText = "")
        {
            var products = await _productService.GetProductsAsync(searchText);
            var productResponse = _mapper.Map<IEnumerable<Entities.Product>, IEnumerable<Models.Product>>(products);
            return Ok(productResponse);
        }

        /// <summary>
        /// Gets a product
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{product-id}")]
        [ProducesResponseType(typeof(Models.Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetProductAsync([FromRoute(Name = "product-id")] int productId)
        {
            var product = await _productService.GetProductAsync(productId);
            var productResponse = _mapper.Map<Entities.Product, Models.Product>(product);
            return Ok(productResponse);
        }

        /// <summary>
        /// Adds a product
        /// </summary>
        /// <param name="request">CreateProductRequest</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Models.Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddProductAsync([FromBody] CreateProductRequest request)
        {
            var prodCategory = _mapper.Map<Models.ProductCategory, Entities.Enums.ProductCategory>(request.ProductCategory);
            var product = await _productService.AddProductAsync(request.SKU, request.AvailableQuantity, request.UnitPrice, prodCategory, request.Name, request.Description, request.ReservedQuantity);
            var productResponse = _mapper.Map<Entities.Product, Models.Product>(product);
            _logger.LogInformation($"Product created with Id: {productResponse.Id}");
            return Ok(productResponse);
        }

        /// <summary>
        /// Updates a product
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <param name="request">UpdateProductRequest</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{product-id}")]
        [ProducesResponseType(typeof(Models.Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProductAsync([FromRoute(Name = "product-id")] int productId, [FromBody] UpdateProductRequest request)
        {
            var prodCategory = _mapper.Map<Models.ProductCategory, Entities.Enums.ProductCategory>(request.ProductCategory);
            var product = await _productService.UpdateProductAsync(productId, request.SKU, request.AvailableQuantity, request.UnitPrice, prodCategory, request.Name, request.Description);
            var productResponse = _mapper.Map<Entities.Product, Models.Product>(product);
            _logger.LogInformation($"Product updated with Id: {productResponse.Id}");
            return Ok(productResponse);
        }

        /// <summary>
        /// Deletes a product. It will soft delete a product marking it as inactive
        /// </summary>
        /// <param name="productId">Product Id</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{product-id}")]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteProductAsync([FromRoute(Name = "product-id")] int productId)
        {
            await _productService.InActiveProductAsync(productId);
            return Ok();
        }
    }
}
