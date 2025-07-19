using Gudena.Data.Entities;

namespace Gudena.Api.Repositories;

public interface ICategoryRepository
{
    Task<ICollection<Category>> GetAllCategoriesAsync();
    Task<ICollection<Category>> GetAllParentCategoriesAsync();
    Task<ICollection<Category>> GetAllChildCategoriesAsync(int parentCategoryId);
    Task<Category> GetCategoryByIdAsync(int id);
    Task<ICollection<Product>> GetProductsByCategoryIdAsync(int categoryId);
}