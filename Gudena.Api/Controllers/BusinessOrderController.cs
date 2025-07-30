using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BusinessOnly")]
public class BusinessOrderController : ControllerBase
{
    private readonly IBusinessOrderService _service;

    public BusinessOrderController(IBusinessOrderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<BusinessOrderDto>>> GetBusinessOrders()
    {
        // Extract Business UserId from claims
        var businessId = User.FindFirst("uid")?.Value;
        var userId = User.FindFirst("uid")?.Value;
        Console.Out.WriteLine($"Business ID: {businessId}");
        if (string.IsNullOrEmpty(businessId))
        {
            return Unauthorized();
        }

        var orders = await _service.GetBusinessOrdersAsync(businessId);
        return Ok(orders);
    }

    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto dto)
    {
        var businessId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(businessId))
            return Unauthorized();

        try
        {
            await _service.UpdateOrderStatusAsync(orderId, businessId, dto.Status);
            return NoContent();
        }
        catch (BusinessOwnershipException ex)
        {
            return Forbid(ex.Message);
        }
        catch (BusinessInvalidStatusChangeException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}