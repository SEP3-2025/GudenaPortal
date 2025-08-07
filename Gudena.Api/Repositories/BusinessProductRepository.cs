using Gudena.Api.Repositories.Interfaces;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories
{
    public class BusinessProductRepository : IBusinessProductRepository
    {
        private readonly AppDbContext _context;

        public BusinessProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId);
        }

        public async Task<Product?> GetProductAsync(int productId, string ownerId)
        {
            return await _context.Products
                .Include(p => p.Media)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId && p.OwnerId == ownerId);
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<object>> GetMyProductsAsync(string ownerId)
        {
            return await _context.Products
                .Where(p => p.OwnerId == ownerId && !p.IsDeleted)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Thumbnail,
                    p.Price,
                    p.Stock,
                    p.Unit,
                    p.ReferenceUrl,
                    p.isReturnable,
                    Category = p.Category != null ? p.Category.Name : null,
                    Media = p.Media.Select(m => m.MediaUrl).ToList()
                })
                .ToListAsync();
        }
    }
}