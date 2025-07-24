using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;

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

    public async Task<IEnumerable<Shipping>> GetShippingsByOrderItemIdAsync(int orderItemId)
    {
        return await _shippingRepository.GetShippingsByOrderItemIdAsync(orderItemId);
    }

    public async Task<Shipping> CreateShippingAsync(Shipping shipping)
    {
        return await _shippingRepository.AddShippingAsync(shipping);
    }

    public async Task<Shipping> UpdateShippingAsync(Shipping shipping)
    {
        return await _shippingRepository.UpdateShippingAsync(shipping);
    }

    public async Task DeleteShippingAsync(int id)
    {
        await _shippingRepository.DeleteShippingAsync(id);
    }
}
