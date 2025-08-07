using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Api.Services;
using Gudena.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gudena.Data.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "BusinessOnly")]

public class BusinessOrderController : ControllerBase
{
    private readonly IBusinessOrderService _service;
    private readonly IPaymentService _paymentService;

    public BusinessOrderController(IBusinessOrderService service, IPaymentService paymentService)
    {
        _service = service;
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<List<BusinessOrderDto>>> GetBusinessOrders()
    {
        // Extract Business UserId from claims
        var businessId = User.FindFirst("uid")?.Value;
        var userId = User.FindFirst("uid")?.Value;
        Console.Out.WriteLine($"Business ID: {businessId}");
        if (string.IsNullOrEmpty(businessId))
        {
            return Unauthorized();
        }

        var orders = await _service.GetBusinessOrdersAsync(businessId);
        return Ok(orders);
    }

    [HttpPut("{orderId}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto dto)
    {
        var businessId = User.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(businessId))
            return Unauthorized();

        try
        {
            await _service.UpdateOrderStatusAsync(orderId, businessId, dto.Status);
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
        
        var order = await _service.GetOrderAsync(userId, id);
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