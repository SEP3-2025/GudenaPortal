using Gudena.Api.Services;
using Gudena.Data.Entities;
using Gudena.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _service;
        private readonly IBasketService _basketService;

        public ShippingController(IShippingService service, IBasketService basketService)
        {
            _service = service;
            _basketService = basketService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<Shipping>> CalculateShipping([FromBody] ShippingRequestDto dto)
        {
            var basket = await _basketService.RetrieveBasketAsync(null, dto.BasketId);
            if (basket == null)
                return NotFound("Basket not found");

            var shipping = await _service.CalculateShippingAsync(
                basket,
                dto.ShippingAddress,
                dto.DeliveryOption ?? "Standard" // default to "Standard" if not set
            );
            return Ok(shipping);
        }

        [HttpGet("{id}/status")]
        public async Task<ActionResult<string>> GetStatus(int id)
        {
            var status = await _service.GetShipmentStatusAsync(id);
            return Ok(status);
        }
    }
}
