using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;
        private readonly IOrderService _orderService;

        public ShippingController(IShippingService shippingService, IOrderService orderService)
        {
            _shippingService = shippingService;
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shipping>> GetShippingAsync(int id)
        {
            var shipping = await _shippingService.GetShippingByIdAsync(id);
            if (shipping == null)
                return NotFound();
            return Ok(shipping);
        }

        [HttpGet("orderitem/{orderItemId}")]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetShippingsByOrderItemIdAsync(int orderItemId)
        {
            var shippings = await _shippingService.GetShippingsByOrderItemIdAsync(orderItemId);
            return Ok(shippings);
        }

        [HttpPost]
        public async Task<ActionResult<Shipping>> CreateShippingAsync([FromBody] ShippingDto dto)
        {
            var shipping = new Shipping
            {
                ShippingAddress = dto.ShippingAddress,
                DeliveryOption = dto.DeliveryOption,
                ShippingNumbers = dto.ShippingNumbers,
                ShippingCost = dto.ShippingCost,
                ShippingStatus = "Ordered",  // default status on create
                OrderItemId = dto.OrderItemId
            };
            var created = await _shippingService.CreateShippingAsync(shipping);
            return CreatedAtAction(nameof(GetShippingAsync), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Shipping>> UpdateShippingAsync(int id, [FromBody] ShippingDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var userId = User.FindFirst("uid")?.Value;
            var shipping = await _shippingService.GetShippingByIdAsync(id);
            if (shipping == null)
                return NotFound();

            // Verify ownership via order
            var orderItem = shipping.OrderItem;
            if (orderItem == null || orderItem.Order == null)
                return BadRequest("Shipping is not linked to a valid order.");

            try
            {
                // Ensure current user owns the order
                var order = await _orderService.GetOrderAsync(userId, orderItem.Order.Id);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("You do not own this shipping/order.");
            }

            // Prevent buyer changes after "Ordered" status
            if (shipping.ShippingStatus != "Ordered")
            {
                return BadRequest("Shipping cannot be modified after it has been processed.");
            }

            // Update fields allowed
            shipping.ShippingAddress = dto.ShippingAddress;
            shipping.DeliveryOption = dto.DeliveryOption;
            shipping.ShippingNumbers = dto.ShippingNumbers;
            shipping.ShippingCost = dto.ShippingCost;
            // You may or may not allow status change here depending on business rules

            var updated = await _shippingService.UpdateShippingAsync(shipping);
            return Ok(updated);
        }
    }
}
