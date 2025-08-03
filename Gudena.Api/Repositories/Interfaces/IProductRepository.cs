using Gudena.Data.Entities;

namespace Gudena.Api.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<IEnumerable<Product>> SearchByNameAsync(string name);
}