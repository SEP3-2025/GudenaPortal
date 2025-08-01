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
    public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
    {
        // Get user
        var userId = User.FindFirst("uid")?.Value;
        try
        {
            Order order = await _orderService.CreateOrderAsync(orderDto, userId);
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
    
    [HttpPost("{id}/refund")]
    [Authorize(Policy = "BusinessOnly")]
    public async Task<IActionResult> IssueRefund(int id, decimal amount)
    {
        var userId = User.FindFirst("uid")?.Value;
        if (userId.IsNullOrEmpty())
            return Unauthorized();

        if (amount <= 0)
        {
            Console.WriteLine($"User {userId} attempted to refund {amount} on order {id}");
            return BadRequest($"Refund amount must be a positive non zero value");
        }
        
        var order = await _orderService.GetOrderAsync(userId, id);
        if (order == null)
            return NotFound("Order not found");

        if (!order.OrderItems.Any(o => o.Product.OwnerId == userId))
            return Forbid(); // User has no relation to the order

        decimal refundableAmount = order.OrderItems.Where(o => o.Product.OwnerId == userId)
            .Sum(o => o.PricePerUnit * o.Quantity);
        var shipping = order.Shippings.FirstOrDefault(s => s.ShippingCost > 0 && s.OrderItems.Any(o => o.Product.OwnerId == userId));
        if (shipping != null)
            refundableAmount += shipping.ShippingCost;
        decimal refundedAmount = order.Payments.Where(p => p.PayingUserId == userId).Sum(p => p.Amount);

        if (refundableAmount - refundedAmount - amount < 0)
            return BadRequest($"Refund exceeds refundable amount {refundableAmount - refundedAmount}");

        var refund = new Payment()
        {
            OrderId = order.Id,
            Amount = amount,
            PaymentMethod = "Refund",
            PayingUserId = userId,
            TransactionDate = DateTime.Now,
            PaymentStatus = "Completed",
            TransactionId = $"GudenaPay-{Guid.NewGuid()}"
        };
        refund = await _paymentService.CreatePaymentAsync(refund);
        
        return Ok(refund);
    }
}