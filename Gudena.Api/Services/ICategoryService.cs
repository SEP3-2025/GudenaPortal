using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public interface ICategoryService
{
    Task<ICollection<Category>> GetAllCategoriesAsync();
    Task<ICollection<Category>> GetAllParentCategoriesAsync();
    Task<ICollection<Category>> GetAllChildCategoriesAsync(int parentCategoryId);
    Task<Category> GetCategoryByIdAsync(int id);
    Task<ICollection<Product>> GetProductsByCategoryIdAsync(int categoryId);
}