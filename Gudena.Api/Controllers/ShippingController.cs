using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;

        public ShippingController(IShippingService shippingService)
        {
            _shippingService = shippingService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shipping>> GetShippingById(int id)
        {
            var shipping = await _shippingService.GetShippingByIdAsync(id);
            if (shipping == null) return NotFound();

            var userId = User.FindFirst("uid")?.Value;
            if (shipping.OrderItems.Any(oi => oi.Order.ApplicationUserId != userId))
                return Unauthorized("You do not have permission to view this shipping.");

            return Ok(shipping);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetUserShippings()
        {
            var userId = User.FindFirst("uid")?.Value;
            var shippings = await _shippingService.GetShippingsByUserIdAsync(userId);
            return Ok(shippings);
        }

        [HttpPost]
        public async Task<ActionResult<Shipping>> CreateShipping([FromBody] ShippingDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            var shipping = new Shipping
            {
                ShippingAddress = dto.ShippingAddress,
                DeliveryOption = dto.DeliveryOption,
                ShippingNumbers = dto.ShippingNumbers,
                ShippingCost = dto.ShippingCost,
                ShippingStatus = dto.ShippingStatus
            };

            var createdShipping = await _shippingService.CreateShippingAsync(shipping, dto.OrderItemIds);
            return CreatedAtAction(nameof(GetShippingById), new { id = createdShipping.Id }, createdShipping);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Shipping>> UpdateShipping(int id, [FromBody] ShippingDto dto)
        {
            var existingShipping = await _shippingService.GetShippingByIdAsync(id);
            if (existingShipping == null) return NotFound();

            var userId = User.FindFirst("uid")?.Value;
            if (existingShipping.OrderItems.Any(oi => oi.Order.ApplicationUserId != userId))
                return Unauthorized("You do not have permission to update this shipping.");

            existingShipping.ShippingAddress = dto.ShippingAddress;
            existingShipping.DeliveryOption = dto.DeliveryOption;
            existingShipping.ShippingNumbers = dto.ShippingNumbers;
            existingShipping.ShippingCost = dto.ShippingCost;
            existingShipping.ShippingStatus = dto.ShippingStatus;

            var updatedShipping = await _shippingService.UpdateShippingAsync(existingShipping, dto.OrderItemIds);
            return Ok(updatedShipping);
        }
    }
}
