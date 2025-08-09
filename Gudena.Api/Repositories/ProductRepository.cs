using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(p => p.Media)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
    {
        var search = name.ToLower();

        return await _context.Products
            .Include(p => p.Media)
            .Where(p => EF.Functions.ILike(p.Name, $"%{search}%"))
            .ToListAsync();
    }
}