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

public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public BasketController(IBasketService basketService, UserManager<ApplicationUser> userManager)
    {
        _basketService = basketService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<BasketItem>> GetBasketAsync()
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        if (userId == null)
            return Unauthorized();
        // Basket -1 retrieves always the basket attached to this user or creates a new one if there is none attached
        var basket = await _basketService.RetrieveBasketAsync(userId, -1);
        if (basket == null)
            return NotFound(); // Shouldn't happen as a new basket is generated if none exists
        return Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<Basket>> CreateBasketAsync(BasketItemDto basketItemDto)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        if (userId == null)
            return Unauthorized();
        try
        {
            Basket basket = await _basketService.AddProductToBasketAsync(userId,
                basketItemDto.ProductId, basketItemDto.Amount);
            return Ok(basket);
        }
        catch (ResourceNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
        catch (BasketExceedsStockException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<ActionResult<Basket>> UpdateBasketAsync(BasketItemDto basketItemDto)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        if (userId == null)
            return Unauthorized();
        try
        {
            var basket = await _basketService.UpdateProductAmountAsync(userId, basketItemDto.ProductId,
                basketItemDto.Amount);
            return Ok(basket);
        }
        catch (ResourceNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
        catch (BasketExceedsStockException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Basket>> DeleteBasketAsync()
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        if (userId == null)
            return Unauthorized();
        try
        {
            await _basketService.DestroyBasketAsync(userId);
            return Ok();
        }
        catch (ResourceNotFoundException e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
}