using System.Security.Claims;
using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;
using Gudena.Api.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager)
    {
        _orderService = orderService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<Order>>> GetOrdersAsync()
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        var orders = await _orderService.GetOrdersAsync(userId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            var order = await _orderService.GetOrderAsync(userId, id);
            return Ok(order);
        }
        catch (ResourceNotFoundException e)
        {
            Console.WriteLine($"Requested order not found: {e}");
            return NotFound();
        }
        catch (UserDoesNotOwnResourceException e)
        {
            Console.WriteLine($"Requested order {id} is not owned by this user {userId}: {e}");
            return Unauthorized();
        }
    }

    [HttpPost]
    public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
    {
        // Get user
        var userId = User.FindFirst("uid")?.Value;
        Order order = await _orderService.CreateOrderAsync(orderDto, userId);
        return Ok(order);
    }

    [HttpPut]
    public async Task<ActionResult<Order>> UpdateOrder(OrderUpdateDto orderDto)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            Order order = await _orderService.UpdateOrderAsync(orderDto, userId);
            return Ok(order);
        }
        catch (ResourceNotFoundException e)
        {
            Console.WriteLine($"Requested order not found: {e}");
            return NotFound();
        }
        catch (UserDoesNotOwnResourceException e)
        {
            Console.WriteLine($"Requested order {orderDto.Id} is not owned by this user {userId}: {e}");
            return Unauthorized();
        }
    }

    [HttpDelete]
    public async Task<IActionResult> CancelOrder(int id)
    {
        // Get userId
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            await _orderService.CancelOrderAsync(userId, id);
            return Ok();
        }
        catch (ResourceNotFoundException e)
        {
            Console.WriteLine($"Requested order not found: {e}");
            return NotFound();
        }
        catch (UserDoesNotOwnResourceException e)
        {
            Console.WriteLine($"Requested order {id} is not owned by this user {userId}: {e}");
            return Unauthorized();
        }
        catch (IncancellableOrderException e)
        {
            Console.WriteLine($"Order cannot be cancelled as it has been processed: {e}");
            return Problem();
        }
    }
}