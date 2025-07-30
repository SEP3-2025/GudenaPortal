using Gudena.Api.DTOs;
using Gudena.Api.Services;
using Gudena.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BusinessOnly")]
public class BusinessProductReturnController : ControllerBase
{
    private readonly IBusinessProductReturnService _service;

    public BusinessProductReturnController(IBusinessProductReturnService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<BusinessProductReturnDto>>> GetProductReturns()
    {
        var businessId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(businessId))
            return Unauthorized();

        var returns = await _service.GetProductReturnsAsync(businessId);
        return Ok(returns);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateProductReturnStatus(int id, [FromBody] UpdateBusinessProductReturnStatusDto dto)
    {
        var businessId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(businessId))
            return Unauthorized();

        try
        {
            var success = await _service.UpdateProductReturnStatusAsync(id, businessId, dto.Status);

            if (!success)
                return NotFound("Product return not found or not owned by this business.");

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}