using Gudena.Api.DTOs;
using Gudena.Api.Services;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly AppDbContext _context;

        public PaymentController(IPaymentService paymentService, AppDbContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> ProcessPayment([FromBody] PaymentRequestDto dto)
        {
            var basket = await _context.Baskets.FindAsync(dto.BasketId);
            if (basket == null)
                return NotFound("Basket not found");

            var payment = await _paymentService.ProcessPaymentAsync(basket, dto.Amount, dto.PaymentMethod);
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
