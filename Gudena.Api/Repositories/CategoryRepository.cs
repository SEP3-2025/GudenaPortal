using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<ICollection<Category>> GetAllCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<ICollection<Category>> GetAllParentCategoriesAsync()
    {
        return await _context.Categories.Where(c => true).ToListAsync(); // TODO: Modify model to add child categories
    }

    public async Task<ICollection<Category>> GetAllChildCategoriesAsync(int parentCategoryId)
    {
        return await _context.Categories.Where(c => true).ToListAsync(); // TODO: Modify model to add child categories
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }
    
    public async Task<ICollection<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await GetCategoryByIdAsync(categoryId);
        return category.Products;
    }
}