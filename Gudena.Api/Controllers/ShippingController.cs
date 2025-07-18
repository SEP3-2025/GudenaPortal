using Gudena.Api.Services;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
    private readonly IShippingService _service;

    public ShippingController(IShippingService service)
    {
        _service = service;
    }

   [HttpPost("calculate")]
    public async Task<ActionResult<Shipping>> CalculateShipping([FromBody] ShippingRequestDto dto)
    {
    // Fetch the real order from the DB using the ID
    var order = await _orderService.GetOrderByIdAsync(dto.OrderId); //I need an order service to fetch the order
    if (order == null)
        return NotFound("Order not found");

    var shipping = await _service.CalculateShippingAsync(order, dto.ShippingAddress);
    return Ok(shipping);
    }   

    [HttpGet("{id}/status")]
    public async Task<ActionResult<string>> GetStatus(int id)
    {
        var status = await _service.GetShipmentStatusAsync(id);
        return Ok(status);
    }
}