using Gudena.Api.DTOs;
using Gudena.Api.Services;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;
        private readonly AppDbContext _context;

        public ShippingController(IShippingService shippingService, AppDbContext context)
        {
            _shippingService = shippingService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Shipping>> CalculateShipping([FromBody] ShippingRequestDto dto)
        {
            var basket = await _context.Baskets.FindAsync(dto.BasketId);
            if (basket == null)
                return NotFound("Basket not found");

            var shipping = await _shippingService.CalculateShippingAsync(basket, dto.ShippingAddress, dto.DeliveryOption);
            return Ok(shipping);
        }

        [HttpGet("{id}/status")]
        public async Task<ActionResult<string>> GetShipmentStatus(int id)
        {
            var status = await _shippingService.GetShipmentStatusAsync(id);
            return Ok(status);
        }
    }
}
