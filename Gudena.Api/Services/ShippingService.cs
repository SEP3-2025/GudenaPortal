using System;
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

        // Example: simple distance calculation (mocked!)
        private double CalculateDistanceKm(string origin, string destination)
        {
            // In production, use real geocoding and Haversine formula!
            // Here, just simulate:
            if (origin == destination) return 1;
            return new Random().Next(5, 501); // random 5-500 km
        }

        public async Task<Shipping> CalculateShippingAsync(Order order, string shippingAddress)
        {
            // Assume warehouse address is hardcoded for demo
            string warehouseAddress = "Gudena Warehouse, Aarhus";
            double distance = CalculateDistanceKm(warehouseAddress, shippingAddress);

            // Shipping cost formula for Standard
            decimal baseCost = 5.0m;
            decimal costPerKm = 0.25m;
            decimal shippingCost = baseCost + (costPerKm * (decimal)distance);

            var shipping = new Shipping
            {
                ShippingAddress = shippingAddress,
                DeliveryOption = "Standard",
                ShippingNumbers = "TRACK" + new Random().Next(100000, 999999),
                ShippingCost = Math.Round(shippingCost, 2)
            };
            await _repository.AddAsync(shipping);
            return shipping;
        }

        public async Task<Shipping> CreateShipmentAsync(Order order, string shippingAddress)
        {
            // Assume warehouse address is hardcoded for demo
            string warehouseAddress = "Gudena Warehouse, Aarhus";
            double distance = CalculateDistanceKm(warehouseAddress, shippingAddress);

            // Shipping cost formula for Express
            decimal baseCost = 10.0m;
            decimal costPerKm = 0.5m;
            decimal shippingCost = baseCost + (costPerKm * (decimal)distance);

            var shipment = new Shipping
            {
                ShippingAddress = shippingAddress,
                DeliveryOption = "Express",
                ShippingNumbers = "EXP" + new Random().Next(100000, 999999),
                ShippingCost = Math.Round(shippingCost, 2)
            };
            await _repository.AddAsync(shipment);
            return shipment;
        }

        public async Task<string> GetShipmentStatusAsync(int shippingId)
        {
            var shipping = await _repository.GetByIdAsync(shippingId);
            return shipping != null ? "In Transit" : "Not Found";
        }
    }
}