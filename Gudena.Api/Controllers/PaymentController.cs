using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPaymentAsync(int id)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id);
        if (payment == null)
            return NotFound();
        return Ok(payment);
    }

    [HttpGet("order/{orderId}")]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByOrderIdAsync(int orderId)
    {
        var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
        return Ok(payments);
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePaymentAsync([FromBody] PaymentDto dto)
    {
        var payment = new Payment
        {
            PaymentMethod = dto.PaymentMethod,
            PaymentStatus = dto.PaymentStatus,
            TransactionDate = dto.TransactionDate,
            Amount = dto.Amount,
            OrderId = dto.OrderId
        };
        var created = await _paymentService.CreatePaymentAsync(payment);
        return CreatedAtAction(nameof(GetPaymentAsync), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Payment>> UpdatePaymentAsync(int id, [FromBody] PaymentDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var payment = new Payment
        {
            Id = dto.Id,
            PaymentMethod = dto.PaymentMethod,
            PaymentStatus = dto.PaymentStatus,
            TransactionDate = dto.TransactionDate,
            Amount = dto.Amount,
            OrderId = dto.OrderId
        };
        var updated = await _paymentService.UpdatePaymentAsync(payment);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaymentAsync(int id)
    {
        await _paymentService.DeletePaymentAsync(id);
        return NoContent();
    }
}
