using Gudena.Data.Entities;

namespace Gudena.Api.Services
{
    public interface IShippingService
    {
        Task<Shipping?> GetShippingByIdAsync(int id);
        Task<Shipping> CreateShippingAsync(Shipping shipping, List<int> orderItemIds);
        Task<Shipping> UpdateShippingAsync(Shipping shipping, List<int> orderItemIds);
    }
}
