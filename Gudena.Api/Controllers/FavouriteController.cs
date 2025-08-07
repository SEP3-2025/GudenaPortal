using System.Security.Claims;
using Gudena.Api.Services;
using Gudena.Api.DTOs;
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

public class FavouriteController : ControllerBase
{
    private readonly IFavouriteService _favouriteService;

    public FavouriteController(IFavouriteService favouriteService)
    {
        _favouriteService = favouriteService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Favourite>>> GetFavourites()
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        List<Favourite> favourites = await _favouriteService.GetFavouritesAsync(userId);
        return Ok(favourites);
    }

    [HttpGet("{productId}")]
    public async Task<ActionResult<bool>> IsFavourite(int productId)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        bool isFavourite = await _favouriteService.IsFavouriteAsync(productId, userId);
        return Ok(isFavourite);
    }

    [HttpPost]
    public async Task<ActionResult<List<Favourite>>> AddToFavourites(int productId)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            List<Favourite> favourites = await _favouriteService.AddToFavouritesAsync(productId, userId);
            return Ok(favourites);
        }
        catch (ArgumentException e) // Is already a favourite
        {
            Console.WriteLine(e);
            return BadRequest();
        }
        catch (DirectoryNotFoundException e) // Product doesn't exist
        {
            Console.WriteLine(e);
            return NotFound();
        }
    }

    [HttpDelete]
    public async Task<ActionResult<List<Favourite>>> RemoveFromFavourites(int productId)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            List<Favourite> favourites = await _favouriteService.RemoveFromFavouritesAsync(productId, userId);
            return Ok(favourites);
        }
        catch (ArgumentException e) // Product is not a favourite
        {
            Console.WriteLine(e);
            return BadRequest();
        }
    }
}