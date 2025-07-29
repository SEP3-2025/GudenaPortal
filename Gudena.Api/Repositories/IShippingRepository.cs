using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gudena.Api.Repositories
{
    public interface IShippingRepository
    {
        Task<Shipping?> GetShippingByIdAsync(int id);
        Task<IEnumerable<Shipping>> GetShippingsByUserIdAsync(string userId);
        Task<Shipping> AddShippingAsync(Shipping shipping, List<int> orderItemIds);
        Task<Shipping> UpdateShippingAsync(Shipping shipping, List<int> orderItemIds);
    }
}
