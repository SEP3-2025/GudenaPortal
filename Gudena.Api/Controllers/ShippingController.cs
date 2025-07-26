using Gudena.Api.Services;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetShipping(int id)
        {
            var shipping = await _shippingService.GetShippingByIdAsync(id);
            if (shipping == null) return NotFound();
            return Ok(shipping);
        }

        [HttpPost]
        public async Task<IActionResult> CreateShipping([FromBody] ShippingCreateDto dto)
        {
            var shipping = new Shipping
            {
                ShippingAddress = dto.ShippingAddress,
                DeliveryOption = dto.DeliveryOption,
                ShippingNumbers = dto.ShippingNumbers,
                ShippingCost = dto.ShippingCost,
                ShippingStatus = dto.ShippingStatus ?? "Ordered"
            };

            var created = await _shippingService.CreateShippingAsync(shipping, dto.OrderItemIds);
            return CreatedAtAction(nameof(GetShipping), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShipping(int id, [FromBody] ShippingCreateDto dto)
        {
            var shipping = new Shipping
            {
                Id = id,
                ShippingAddress = dto.ShippingAddress,
                DeliveryOption = dto.DeliveryOption,
                ShippingNumbers = dto.ShippingNumbers,
                ShippingCost = dto.ShippingCost,
                ShippingStatus = dto.ShippingStatus ?? "Ordered"
            };

            var updated = await _shippingService.UpdateShippingAsync(shipping, dto.OrderItemIds);
            return Ok(updated);
        }
    }

    public class ShippingCreateDto
    {
        public string ShippingAddress { get; set; } = null!;
        public string DeliveryOption { get; set; } = null!;
        public string ShippingNumbers { get; set; } = null!;
        public decimal ShippingCost { get; set; }
        public string? ShippingStatus { get; set; }
        public List<int> OrderItemIds { get; set; } = new List<int>();
    }
}
