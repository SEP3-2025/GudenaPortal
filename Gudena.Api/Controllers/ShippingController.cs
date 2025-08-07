using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Gudena.Services;
using Microsoft.AspNetCore.Authorization;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingService _shippingService;
        private readonly IAccountDetailsService _accountDetailsService;
        private readonly IOrderItemService _orderItemService;
        private readonly IBasketService _basketService;

        public ShippingController(IShippingService shippingService, IAccountDetailsService accountDetailsService, IOrderItemService orderItemService, IBasketService basketService)
        {
            _shippingService = shippingService;
            _accountDetailsService = accountDetailsService;
            _orderItemService = orderItemService;
            _basketService = basketService;
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
        public async Task<ActionResult<Shipping>> CreateShipping([FromBody] CreateShippingDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null) return Unauthorized();

            // Buyer account
            var buyer = await _accountDetailsService.GetAccountDetailsForUserAsync(userId);
            if (buyer == null)
                return BadRequest("Account details not found.");

            /*// Get product owner account details
            var orderItemId = dto.OrderItemIds.First();
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(orderItemId);
            if (orderItem == null)
                return BadRequest("Order item not found.");

            var productOwnerId = orderItem.Product.OwnerId; // Make sure Product has OwnerId
            var ownerAccount = await _accountDetailsService.GetAccountDetailsForUserAsync(productOwnerId);
            if (ownerAccount == null)
                return BadRequest("Product owner account details not found.");
            */
            await _shippingService.CleanUpPreviews(userId);
            
            var ownerInformation = await _basketService.GetBusinessDetailsForBasketAsync(userId);
            List<Shipping> shippings = new List<Shipping>();
            
            foreach (var business in ownerInformation)
            {
                // Calculate shipping cost (origin = product owner, destination = buyer)
                double shippingCost = await GudenaShippingClient.GetShippingCostAsync(
                    originCountry: business.Country,
                    originPostalCode: business.PostalCode,
                    destCountry: buyer.Country,
                    destPostalCode: buyer.PostalCode);
                if (shippingCost < 0)
                    return StatusCode(500, "Error retrieving shipping cost from GudenaShipping service.");

                var shipping = new Shipping
                {
                    City = buyer.City,
                    Street = buyer.Street,
                    PostalCode = buyer.PostalCode,
                    Country = buyer.Country,
                    DeliveryOption = dto.DeliveryOption,
                    ShippingNumbers = Guid.NewGuid().ToString(),
                    ShippingCost = Convert.ToDecimal(shippingCost),
                    ShippingStatus = "Preview",
                    ApplicationUserId = userId,
                    BusinessUserId = business.ApplicationUserId
                };
                shippings.Add(await _shippingService.CreateShippingAsync(shipping, new List<int>()));
            }
            
            return Ok(shippings);
        }


        [HttpPut("{id}")]
        [Authorize(Policy = "BusinessOnly")] 
        public async Task<ActionResult<Shipping>> UpdateShipping(int id, [FromBody] UpdateShippingDto dto)
        {
            var existingShipping = await _shippingService.GetShippingByIdAsync(id);
            if (existingShipping == null) return NotFound();

            var userId = User.FindFirst("uid")?.Value;
            if (existingShipping.OrderItems.Any(oi => oi.Order.ApplicationUserId != userId))
                return Unauthorized("You do not have permission to update this shipping.");
            
            var account = await _accountDetailsService.GetAccountDetailsForUserAsync(userId);
            if (account == null)
                return BadRequest("Account details not found.");
            
            double shippingCost = await GudenaShippingClient.GetShippingCostAsync(
                account.Country, account.PostalCode,
                dto.Country, dto.PostalCode);
            
            if (shippingCost < 0)
                return StatusCode(500, "Error retrieving shipping cost from GudenaShipping service.");

            var convertedShippingCost = Convert.ToDecimal(shippingCost);

            existingShipping.City = dto.City;
            existingShipping.Street = dto.Street;
            existingShipping.PostalCode = dto.PostalCode;
            existingShipping.Country = dto.Country;
            existingShipping.DeliveryOption = dto.DeliveryOption;
            existingShipping.ShippingNumbers = dto.ShippingNumbers;
            existingShipping.ShippingCost = Convert.ToDecimal(shippingCost);
            existingShipping.ShippingStatus = dto.ShippingStatus;

            var updatedShipping = await _shippingService.UpdateShippingAsync(existingShipping, dto.OrderItemIds);
            return Ok(updatedShipping);
        }
    }
}
