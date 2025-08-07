using System.ComponentModel.Design;
using System.Security.Claims;
using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BuyerOnly")]

public class WarrantyClaimController : ControllerBase
{
    private readonly IWarrantyClaimService _warrantyClaimService;

    public WarrantyClaimController(IWarrantyClaimService warrantyClaimService)
    {
        _warrantyClaimService = warrantyClaimService;
    }

    [HttpGet]
    public async Task<ActionResult<List<WarrantyClaim>>> GetAllWarrantyClaimsAsync()
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        List<WarrantyClaim> warrantyClaims = await _warrantyClaimService.GetAsync(userId);
        return Ok(warrantyClaims);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WarrantyClaim>> GetWarrantyClaimAsync(int id)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        WarrantyClaim warrantyClaim = await _warrantyClaimService.GetByIdAsync(userId, id);
        return Ok(warrantyClaim);
    }

    [HttpPost]
    public async Task<ActionResult<WarrantyClaim>> SubmitWarrantyClaimAsync(WarrantyClaimDto warrantyClaimDto)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            WarrantyClaim warrantyClaim = await _warrantyClaimService.CreateAsync(warrantyClaimDto, userId);
            return Ok(warrantyClaim);
        }
        catch (ResourceNotFoundException e) // OrderItem not found
        {
            Console.WriteLine(e);
            return NotFound();
        }
        catch (WarrantyAlreadyClaimedException e) // A warranty claim has already been submitted for this OrderItem
        {
            Console.WriteLine(e);
            return Problem();
        }
        catch (UserDoesNotOwnResourceException e) // User does not own the OrderItem
        {
            Console.WriteLine(e);
            return Unauthorized();
        }
    }
}