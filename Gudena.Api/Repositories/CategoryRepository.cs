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
        return await _context.Categories
            .Include(c => c.Products)
            .ToListAsync();
    }

    public async Task<ICollection<Category>> GetRootCategoriesAsync()
    {
        return await _context.Categories
            .Include(c => c.Products)
            .Where(c => c.ParentCategory == null).ToListAsync();
    }

    public async Task<ICollection<Category>> GetAllChildCategoriesAsync(int parentCategoryId)
    {
        return await _context.Categories
            .Include(c => c.Products)
            .Where(c => c.ParentCategoryId == parentCategoryId).ToListAsync();
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<ICollection<Product>> GetProductsByCategoryIdAsync(int categoryId)
    {
        var category = await GetCategoryByIdAsync(categoryId);
        return category.Products;
    }
}