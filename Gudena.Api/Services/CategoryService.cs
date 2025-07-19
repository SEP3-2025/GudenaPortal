using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<ICollection<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllCategoriesAsync();
    }

    public async Task<ICollection<Category>> GetRootCategoriesAsync()
    {
        return await _categoryRepository.GetRootategoriesAsync();
    }

    public async Task<ICollection<Category>> GetAllChildCategoriesAsync(int parentCategoryId)
    {
        return await _categoryRepository.GetAllChildCategoriesAsync(parentCategoryId);
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetCategoryByIdAsync(id);
    }

    public async Task<ICollection<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        return await _categoryRepository.GetProductsByCategoryIdAsync(categoryId);
    }
}