using Gudena.Api.Services;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuyerProductsController : ControllerBase
{
    private readonly IBuyerProductService _service;

    public BuyerProductsController(IBuyerProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _service.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _service.GetProductByIdAsync(id);
        if (product == null)
            return NotFound();

        return Ok(product);
    }

    // New endpoint for search by name
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Search term cannot be empty.");

        var products = await _service.SearchProductsByNameAsync(name);

        if (products == null || !products.Any())
            return NotFound("No products found matching the search term.");

        return Ok(products);
    }
}