using Gudena.Data.Entities;

namespace Gudena.Api.Repositories;

public interface IShippingRepository
{
    Task<Shipping?> GetShippingByIdAsync(int id);
    Task<IEnumerable<Shipping>> GetShippingsByOrderItemIdAsync(int orderItemId);
    Task<Shipping> AddShippingAsync(Shipping shipping);
    Task<Shipping> UpdateShippingAsync(Shipping shipping);
    Task DeleteShippingAsync(int id);
}
