using Gudena.Api.Repositories;
using Gudena.Data.Entities;
using System.Threading.Tasks;

namespace Gudena.Api.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository _repository;

        public ShippingService(IShippingRepository repository)
        {
            _repository = repository;
        }

        public async Task<Shipping> CalculateShippingAsync(Basket basket, string shippingAddress, string deliveryOption = "Standard")
        {
            var shipping = new Shipping
            {
                BasketId = basket.Id,
                ShippingAddress = shippingAddress,
                DeliveryOption = deliveryOption,
                ShippingNumbers = "DUMMYTRACK123456",
                ShippingCost = 9.99m, // dummy value
                Basket = basket
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
