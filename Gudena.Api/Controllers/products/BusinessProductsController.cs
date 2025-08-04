using Gudena.Api.DTOs;
using Gudena.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/business/products")]
    [Authorize(Policy = "BusinessOnly")]
    public class BusinessProductController : ControllerBase
    {
        private readonly IBusinessProductService _service;

        public BusinessProductController(IBusinessProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto dto)
        {
            var userId = User.FindFirstValue("uid");
            var result = await _service.CreateProductAsync(userId, dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductCreateDto dto)
        {
            var userId = User.FindFirstValue("uid");
            var result = await _service.UpdateProductAsync(userId, id, dto);
            if (result.Contains("not found")) return NotFound(result);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var userId = User.FindFirstValue("uid");
            var result = await _service.DeleteProductAsync(userId, id);
            if (result.Contains("not found")) return NotFound(result);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProducts()
        {
            var userId = User.FindFirstValue("uid");
            var result = await _service.GetMyProductsAsync(userId);
            return Ok(result);
        } 
        
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var userId = User.FindFirstValue("uid");
            var result = await _service.GetProduct(userId, productId);
            return Ok(result);
        }
    }
}