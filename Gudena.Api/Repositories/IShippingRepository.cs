using Gudena.Data.Entities;

namespace Gudena.Api.Repositories
{
    public interface IShippingRepository
    {
        Task AddAsync(Shipping shipping);
        Task<Shipping?> GetByIdAsync(int id);
    }
}