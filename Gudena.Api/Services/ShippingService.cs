using System;
using System.Linq;
using System.Threading.Tasks;
using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository _repository;

        public ShippingService(IShippingRepository repository)
        {
            _repository = repository;
        }

        private double CalculateDistanceKm(string origin, string destination)
        {
            if (origin == destination) return 1;
            return new Random().Next(5, 501); // need GoogleMaps API to calculate distance so put random 5-500 km
        }

        public async Task<Shipping> CalculateShippingAsync(
            Basket basket, 
            string shippingAddress, 
            string deliveryOption = "Standard")
        {
            string warehouseAddress = "Gudena Warehouse, Horsens";
            double distance = CalculateDistanceKm(warehouseAddress, shippingAddress);

            // Safety: handle if BasketItems is null
            int totalItems = basket.BasketItems?.Sum(bi => bi.Amount) ?? 0;

            // Shipping cost formula
            decimal baseCost = deliveryOption == "Express" ? 10.0m : 5.0m;
            decimal costPerKm = deliveryOption == "Express" ? 0.5m : 0.25m;
            decimal itemCost = totalItems * 0.50m; // 0.50 per item
            decimal shippingCost = baseCost + (costPerKm * (decimal)distance) + itemCost;

            var shipping = new Shipping
            {
                ShippingAddress = shippingAddress,
                DeliveryOption = deliveryOption,
                ShippingNumbers = (deliveryOption == "Express" ? "EXP" : "TRACK") + new Random().Next(100000, 999999),
                ShippingCost = Math.Round(shippingCost, 2),
            };

            await _repository.AddAsync(shipping);
            return shipping;
        }

        public async Task<string> GetShipmentStatusAsync(int shippingId)
        {
            var shipping = await _repository.GetByIdAsync(shippingId);
            return shipping != null ? "In Transit" : "Not Found";
        }
    }
}
