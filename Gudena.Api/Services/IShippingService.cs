using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public interface IShippingService
{
    Task<Shipping?> GetShippingByIdAsync(int id);
    Task<IEnumerable<Shipping>> GetShippingsByOrderItemIdAsync(int orderItemId);
    Task<Shipping> CreateShippingAsync(Shipping shipping);
    Task<Shipping> UpdateShippingAsync(Shipping shipping);
    Task DeleteShippingAsync(int id);
}
