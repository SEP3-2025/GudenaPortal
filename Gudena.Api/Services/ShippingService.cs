using Gudena.Api.Repositories;
using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<List<Shipping>> GetShippingsByUserIdAsync(string userId)
        {
            return await _shippingRepository.GetShippingsByUserIdAsync(userId);
        }

        public async Task<Shipping> CreateShippingAsync(Shipping shipping, List<int> orderItemIds)
        {
            return await _shippingRepository.AddShippingAsync(shipping, orderItemIds);
        }

        public async Task<Shipping> UpdateShippingAsync(Shipping shipping, List<int> orderItemIds)
        {
            return await _shippingRepository.UpdateShippingAsync(shipping, orderItemIds);
        }

        public async Task<List<Shipping>> RetrievePreviews(string userId)
        {
            return await _shippingRepository.RetrievePreviews(userId);
        }

        public async Task CleanUpPreviews(string userId)
        {
            await _shippingRepository.CleanUpPreviews(userId);
        }
    }
}
