using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public interface IBuyerProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> SearchProductsByNameAsync(string name);
}