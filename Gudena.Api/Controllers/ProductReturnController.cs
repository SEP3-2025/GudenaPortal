using System.Security.Claims;
using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductReturnController : ControllerBase
{
    private readonly IProductReturnService _productReturnService;

    public ProductReturnController(IProductReturnService productReturnService)
    {
        _productReturnService = productReturnService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductReturn>>> GetAllProductReturnsAsync()
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        List<ProductReturn> productReturns = await _productReturnService.GetAsync(userId);
        return Ok(productReturns);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductReturn>> GetProductReturnAsync(int id)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        ProductReturn productReturn = await _productReturnService.GetByIdAsync(userId, id);
        return Ok(productReturn);
    }

    [HttpPost]
    public async Task<ActionResult<ProductReturn>> RequestProductReturnAsync(ProductReturnDto productReturnDto)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            ProductReturn productReturn = await _productReturnService.CreateAsync(productReturnDto, userId);
            return Ok(productReturn);
        }
        catch (ResourceNotFoundException e) // OrderItem not found
        {
            Console.WriteLine(e);
            return NotFound();
        }
        catch (ProductAlreadyReturnedException e) // Return has already been requested for this OrderItem
        {
            Console.WriteLine(e);
            return Problem();
        }
        catch (NonReturnableProductException e)
        {
            Console.WriteLine(e);
            return BadRequest();
        }
        catch (UserDoesNotOwnResourceException e) // User does not own the OrderItem
        {
            Console.WriteLine(e);
            return Unauthorized();
        }
    }
}