using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Gudena.Api.Controllers
{
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
        public async Task<ActionResult<Payment>> GetPaymentById(int id)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null) return Unauthorized();

            var payment = await _paymentService.GetPaymentByIdAndUserIdAsync(id, userId);
            if (payment == null) return NotFound();

            return Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByOrderId(int orderId)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null) return Unauthorized();

            var payments = await _paymentService.GetPaymentsByOrderIdAndUserIdAsync(orderId, userId);
            return Ok(payments);
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] PaymentDto dto, [FromServices] IConfiguration configuration)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null) return Unauthorized();

            var paymentServerResponse = await GudenaPaymentClient.ProcessPaymentAsync(dto.CreditCard);

            if (paymentServerResponse != "APPROVED")
            {
                await Console.Error.WriteLineAsync($"Payment declined: {paymentServerResponse}");
                return BadRequest("Payment was declined by the PaymentServer");
            }

            var payment = new Payment
            {
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = "Completed",
                TransactionDate = DateTime.UtcNow,
                Amount = dto.Amount,
                TransactionId = Guid.NewGuid().ToString(),
                PayingUserId = userId
            };

            var createdPayment = await _paymentService.CreatePaymentAsync(payment);
            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.Id }, createdPayment);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "BusinessOnly")] 
        public async Task<ActionResult<Payment>> UpdatePayment(int id, [FromBody] PaymentDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null) return Unauthorized();

            var existingPayment = await _paymentService.GetPaymentByIdAndUserIdAsync(id, userId);
            if (existingPayment == null) return NotFound();

            existingPayment.PaymentMethod = dto.PaymentMethod;
            existingPayment.PaymentStatus = dto.PaymentStatus;
            existingPayment.TransactionDate = dto.TransactionDate;
            existingPayment.Amount = dto.Amount;
            existingPayment.OrderId = dto.OrderId;

            var updatedPayment = await _paymentService.UpdatePaymentAsync(existingPayment);
            return Ok(updatedPayment);
        }
    }
}
