using System.Security.Claims;
using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;
using Gudena.Api.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BuyerOnly")]

public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IPaymentService _paymentService;
    
    public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager, IPaymentService paymentService)
    {
        _orderService = orderService;
        _userManager = userManager;
        _paymentService = paymentService;
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
    public async Task<ActionResult<Order>> CreateOrder()
    {
        // Get user
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            Order order = await _orderService.CreateOrderAsync(userId);
            return Ok(order);
        }
        catch (OrderExceedsStockException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        catch (UnpaidException e)
        {
            Console.WriteLine(e);
            return StatusCode(402, e.Message); // Payment Required
        }
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
        catch (OrderExceedsStockException e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
        catch (UnpaidException e)
        {
            Console.WriteLine(e);
            return StatusCode(402, e.Message); // Payment required
        }
        catch (CannotModifyOrderException e)
        {
            Console.WriteLine(e);
            return Problem();
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