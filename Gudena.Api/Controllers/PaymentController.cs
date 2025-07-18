using Gudena.Api.DTOs;
using Gudena.Api.Services;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService; // If you need to fetch order info

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> ProcessPayment([FromBody] PaymentRequestDto dto)
        {
            var order = await _orderService.GetOrderByIdAsync(dto.OrderId);
            if (order == null)
                return NotFound("Order not found");

            var payment = await _paymentService.ProcessPaymentAsync(order, dto.Amount, dto.PaymentMethod);
            return Ok(payment);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();

            return Ok(payment);
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByOrderId(int orderId)
        {
            var payments = await _paymentService.GetPaymentsByOrderIdAsync(orderId);
            return Ok(payments);
        }
    }
}
