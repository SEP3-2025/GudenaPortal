using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gudena.Api.Services
{
    public interface IShippingService
    {
        Task<Shipping?> GetShippingByIdAsync(int id);
        Task<List<Shipping>> GetShippingsByUserIdAsync(string userId);
        Task<Shipping> CreateShippingAsync(Shipping shipping, List<int> orderItemIds);
        Task<Shipping> UpdateShippingAsync(Shipping shipping, List<int> orderItemIds);
        Task<List<Shipping>> RetrievePreviews(string userId);
        Task CleanUpPreviews(string userId);
    }
}
