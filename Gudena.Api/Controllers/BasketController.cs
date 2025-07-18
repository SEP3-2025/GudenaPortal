using System.Security.Claims;
using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    
    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet]
    public async Task<ActionResult<BasketItem>> GetBasketAsync(int id)
    {
        // Get userId
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var basket = await _basketService.RetrieveBasketAsync(userId, id);
        if (basket == null)
            return NotFound(); // Shouldn't happen as a new basket is generated if none exists
        return Ok(basket);
    }

    [HttpPost]
    public async Task<ActionResult<Basket>> CreateBasketAsync(BasketItemDto basketItemDto)
    {
        try
        {
            Basket basket = await _basketService.AddProductToBasketAsync(basketItemDto.BasketId, basketItemDto.ProductId, basketItemDto.Amount);
            return Ok(basket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpPut]
    public async Task<ActionResult<Basket>> UpdateBasketAsync(BasketItemDto basketItemDto)
    {
        try
        {
            var basket = await _basketService.UpdateProductAmountAsync(basketItemDto.BasketId, basketItemDto.ProductId,
                basketItemDto.Amount);
            return Ok(basket);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpDelete]
    public async Task<ActionResult<Basket>> DeleteBasketAsync(int id)
    {
        try
        {
            await _basketService.DestroyBasketAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }
}