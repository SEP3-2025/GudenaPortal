using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShippingController : ControllerBase
{
    private readonly IShippingService _shippingService;

    public ShippingController(IShippingService shippingService)
    {
        _shippingService = shippingService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Shipping>> GetShippingAsync(int id)
    {
        var shipping = await _shippingService.GetShippingByIdAsync(id);
        if (shipping == null)
            return NotFound();
        return Ok(shipping);
    }

    [HttpGet("orderitem/{orderItemId}")]
    public async Task<ActionResult<IEnumerable<Shipping>>> GetShippingsByOrderItemIdAsync(int orderItemId)
    {
        var shippings = await _shippingService.GetShippingsByOrderItemIdAsync(orderItemId);
        return Ok(shippings);
    }

    [HttpPost]
    public async Task<ActionResult<Shipping>> CreateShippingAsync([FromBody] ShippingDto dto)
    {
        var shipping = new Shipping
        {
            ShippingAddress = dto.ShippingAddress,
            DeliveryOption = dto.DeliveryOption,
            ShippingNumbers = dto.ShippingNumbers,
            ShippingCost = dto.ShippingCost,
            OrderItemId = dto.OrderItemId
        };
        var created = await _shippingService.CreateShippingAsync(shipping);
        return CreatedAtAction(nameof(GetShippingAsync), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Shipping>> UpdateShippingAsync(int id, [FromBody] ShippingDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var shipping = new Shipping
        {
            Id = dto.Id,
            ShippingAddress = dto.ShippingAddress,
            DeliveryOption = dto.DeliveryOption,
            ShippingNumbers = dto.ShippingNumbers,
            ShippingCost = dto.ShippingCost,
            OrderItemId = dto.OrderItemId
        };
        var updated = await _shippingService.UpdateShippingAsync(shipping);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShippingAsync(int id)
    {
        await _shippingService.DeleteShippingAsync(id);
        return NoContent();
    }
}
