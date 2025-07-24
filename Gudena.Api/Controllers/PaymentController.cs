using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
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

        [HttpPatch("{id}/refund")]
        public async Task<IActionResult> RefundPaymentAsync(int id)
        {
            var userId = User.FindFirst("uid")?.Value;
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            if (payment.OrderId == null)
                return BadRequest("This payment is not associated with any order.");

            if (payment.PaymentStatus == "Refunded")
                return BadRequest("This payment has already been refunded.");

            try
            {
                // Throws exception if user doesn't own order
                var order = await _orderService.GetOrderAsync(userId, payment.OrderId.Value);

                payment.PaymentStatus = "Refunded";
                await _paymentService.UpdatePaymentAsync(payment);

                return Ok(payment);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You do not own this order/payment.");
            }
        }
    }
}
