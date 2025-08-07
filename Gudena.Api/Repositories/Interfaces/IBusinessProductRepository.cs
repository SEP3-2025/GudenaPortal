using Gudena.Data.Entities;

namespace Gudena.Api.Repositories.Interfaces;

public interface IBusinessProductRepository
{
    Task<Category?> GetCategoryAsync(int categoryId);
    Task<Product?> GetProductAsync(int productId, string ownerId);
    Task AddProductAsync(Product product);
    Task SaveChangesAsync();
    Task<IEnumerable<object>> GetMyProductsAsync(string ownerId);
}