using Gudena.Api.Services;
using Gudena.Api.DTOs;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Gudena.Services;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public ShippingController(
            IShippingService shippingService,
            IAccountDetailsService accountDetailsService,
            IOrderItemService orderItemService,
            IBasketService basketService)
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
        [Authorize(Policy = "BuyerOnly")]
        public async Task<ActionResult<IEnumerable<Shipping>>> GetUserShippings()
        {
            var userId = User.FindFirst("uid")?.Value;
            var shippings = await _shippingService.GetShippingsByUserIdAsync(userId);
            return Ok(shippings);
        }

        [HttpPost]
        [Authorize(Policy = "BuyerOnly")]
        public async Task<ActionResult<IEnumerable<Shipping>>> CreateShipping([FromBody] CreateShippingDto dto)
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null) return Unauthorized();

            // Buyer account
            var buyer = await _accountDetailsService.GetAccountDetailsForUserAsync(userId);
            if (buyer == null)
                return BadRequest("Account details not found.");

            await _shippingService.CleanUpPreviews(userId);

            var ownerInformation = await _basketService.GetBusinessDetailsForBasketAsync(userId);
            List<Shipping> shippings = new List<Shipping>();

            foreach (var business in ownerInformation)
            {
                decimal shippingCost;
                try
                {
                    shippingCost = await GetShippingCostAsync(
                        originCountry: business.Country,
                        originPostalCode: business.PostalCode,
                        destCountry: buyer.Country,
                        destPostalCode: buyer.PostalCode);
                }
                catch (InvalidOperationException)
                {
                    return StatusCode(500, "Error retrieving shipping cost from GudenaShipping service.");
                }

                var shipping = new Shipping
                {
                    City = buyer.City,
                    Street = buyer.Street,
                    PostalCode = buyer.PostalCode,
                    Country = buyer.Country,
                    DeliveryOption = dto.DeliveryOption,
                    ShippingNumbers = Guid.NewGuid().ToString(),
                    ShippingCost = shippingCost,
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

            decimal convertedShippingCost;
            try
            {
                convertedShippingCost = await GetShippingCostAsync(
                    originCountry: account.Country,
                    originPostalCode: account.PostalCode,
                    destCountry: dto.Country,
                    destPostalCode: dto.PostalCode);
            }
            catch (InvalidOperationException)
            {
                return StatusCode(500, "Error retrieving shipping cost from GudenaShipping service.");
            }

            existingShipping.City = dto.City;
            existingShipping.Street = dto.Street;
            existingShipping.PostalCode = dto.PostalCode;
            existingShipping.Country = dto.Country;
            existingShipping.DeliveryOption = dto.DeliveryOption;
            existingShipping.ShippingNumbers = dto.ShippingNumbers;
            existingShipping.ShippingCost = convertedShippingCost;
            existingShipping.ShippingStatus = dto.ShippingStatus;

            var updatedShipping = await _shippingService.UpdateShippingAsync(existingShipping, dto.OrderItemIds);
            return Ok(updatedShipping);
        }
        
        [HttpGet("preview-costs")]
        [Authorize(Policy = "BuyerOnly")]
        public async Task<ActionResult<IEnumerable<ShippingCostPreviewDto>>> GetPreviewShippingCosts()
        {
            var userId = User.FindFirst("uid")?.Value;
            if (userId == null) return Unauthorized();

            // Buyer (destination)
            var buyer = await _accountDetailsService.GetAccountDetailsForUserAsync(userId);
            if (buyer == null) return BadRequest("Account details not found.");

            if (string.IsNullOrWhiteSpace(buyer.Country) || string.IsNullOrWhiteSpace(buyer.PostalCode))
                return BadRequest("Buyer address is incomplete (country/postal code required).");

            // Sellers (origins) with items in the buyer's basket
            var ownerInformation = await _basketService.GetBusinessDetailsForBasketAsync(userId);
            if (ownerInformation == null || !ownerInformation.Any())
                return Ok(Array.Empty<ShippingCostPreviewDto>());

            var results = new List<ShippingCostPreviewDto>(ownerInformation.Count);

            foreach (var business in ownerInformation)
            {
                var businessName =
                    !string.IsNullOrWhiteSpace(business.CompanyName)
                        ? business.CompanyName
                        : (!string.IsNullOrWhiteSpace(business.FirstName) ||
                           !string.IsNullOrWhiteSpace(business.LastName))
                            ? $"{business.FirstName} {business.LastName}".Trim()
                            : business.ApplicationUserId; // last-resort fallback
                
                if (string.IsNullOrWhiteSpace(business.Country) || string.IsNullOrWhiteSpace(business.PostalCode))
                {
                    results.Add(new ShippingCostPreviewDto(
                        BusinessName: businessName,
                        OriginCountry: business.Country ?? "",
                        OriginPostalCode: business.PostalCode ?? "",
                        DestinationCountry: buyer.Country,
                        DestinationPostalCode: buyer.PostalCode,
                        Cost: null,
                        Message: "Seller address incomplete"
                    ));
                    continue;
                }

                try
                {
                    var cost = await GetShippingCostAsync(
                        originCountry: business.Country,
                        originPostalCode: business.PostalCode,
                        destCountry: buyer.Country,
                        destPostalCode: buyer.PostalCode);

                    results.Add(new ShippingCostPreviewDto(
                        BusinessName: businessName,
                        OriginCountry: business.Country,
                        OriginPostalCode: business.PostalCode,
                        DestinationCountry: buyer.Country,
                        DestinationPostalCode: buyer.PostalCode,
                        Cost: cost,
                        Message: "OK"
                    ));
                }
                catch
                {
                    results.Add(new ShippingCostPreviewDto(
                        BusinessName: businessName,
                        OriginCountry: business.Country,
                        OriginPostalCode: business.PostalCode,
                        DestinationCountry: buyer.Country,
                        DestinationPostalCode: buyer.PostalCode,
                        Cost: null,
                        Message: "GudenaShipping unavailable"
                    ));
                }
            }

            return Ok(results);
        }

        
        private static async Task<decimal> GetShippingCostAsync(
            string originCountry,
            string originPostalCode,
            string destCountry,
            string destPostalCode)
        {
            double cost = await GudenaShippingClient.GetShippingCostAsync(
                originCountry: originCountry,
                originPostalCode: originPostalCode,
                destCountry: destCountry,
                destPostalCode: destPostalCode);

            if (cost < 0)
                throw new InvalidOperationException("GudenaShipping returned an invalid cost.");

            return Convert.ToDecimal(cost);
        }
    }
}