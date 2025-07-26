using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services
{
    public class ShippingService : IShippingService
    {
        private readonly IShippingRepository _shippingRepository;

        public ShippingService(IShippingRepository shippingRepository)
        {
            _shippingRepository = shippingRepository;
        }

        public async Task<Shipping?> GetShippingByIdAsync(int id)
        {
            return await _shippingRepository.GetShippingByIdAsync(id);
        }

        public async Task<Shipping> CreateShippingAsync(Shipping shipping, List<int> orderItemIds)
        {
            return await _shippingRepository.AddShippingAsync(shipping, orderItemIds);
        }

        public async Task<Shipping> UpdateShippingAsync(Shipping shipping, List<int> orderItemIds)
        {
            return await _shippingRepository.UpdateShippingAsync(shipping, orderItemIds);
        }
    }
}
