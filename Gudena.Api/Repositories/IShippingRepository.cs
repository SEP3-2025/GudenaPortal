using Gudena.Data.Entities;

namespace Gudena.Api.Repositories
{
    public interface IShippingRepository
    {
        Task<Shipping?> GetShippingByIdAsync(int id);
        Task<Shipping> AddShippingAsync(Shipping shipping, List<int> orderItemIds);
        Task<Shipping> UpdateShippingAsync(Shipping shipping, List<int> orderItemIds);
    }
}
