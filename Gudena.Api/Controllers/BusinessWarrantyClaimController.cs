using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Api.Services;
using Gudena.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BusinessOnly")]

public class BusinessWarrantyClaimController : ControllerBase
{
    private readonly IBusinessWarrantyClaimService _service;

    public BusinessWarrantyClaimController(IBusinessWarrantyClaimService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<BusinessWarrantyClaimDto>>> GetWarrantyClaims()
    {
        var businessId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(businessId))
            return Unauthorized();

        var claims = await _service.GetWarrantyClaimsAsync(businessId);
        return Ok(claims);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateWarrantyClaimStatus(int id, [FromBody] UpdateBusinessWarrantyClaimStatusDto dto)
    {
        var businessId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(businessId))
            return Unauthorized();

        try
        {
            await _service.UpdateWarrantyClaimStatusAsync(id, businessId, dto.Status);
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