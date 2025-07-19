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
        private readonly IBasketService _basketService;

        public PaymentController(IPaymentService paymentService, IBasketService basketService)
        {
            _paymentService = paymentService;
            _basketService = basketService;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> ProcessPayment([FromBody] PaymentRequestDto dto)
        {
            var basket = await _basketService.RetrieveBasketAsync(null, dto.BasketId);
            if (basket == null)
                return NotFound("Basket not found");

            var payment = await _paymentService.ProcessPaymentAsync(basket, dto.Amount, dto.PaymentMethod);
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

        [HttpGet("basket/{basketId}")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPaymentsByBasketId(int basketId)
        {
            var payments = await _paymentService.GetPaymentsByBasketIdAsync(basketId);
            return Ok(payments);
        }
    }
}
