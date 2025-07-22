using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class FavouriteRepository : IFavouriteRepository
{
    private readonly AppDbContext _context;
    private readonly IProductRepository _productRepository;

    public FavouriteRepository(AppDbContext context, IProductRepository productRepository)
    {
        _context = context;
        _productRepository = productRepository;
    }
    
    public async Task<List<Favourite>> GetFavouritesAsync(string userId)
    {
        return await _context.Favourites
            .Include(f => f.Product)
            .Where(f => f.ApplicationUserId == userId)
            .ToListAsync();
    }

    public async Task<bool> IsFavouriteAsync(int productId, string userId)
    {
        return await _context.Favourites.AnyAsync(f => f.ApplicationUserId == userId && f.ProductId == productId);
    }

    public async Task<List<Favourite>> AddToFavouritesAsync(int productId, string userId)
    {
        if (await IsFavouriteAsync(productId, userId))
        {
            Console.WriteLine($"Product {productId} is already in favourites");
            throw new ArgumentException("Product is already in favourites");
        }
        Product? product = await _context.Products.FindAsync(productId);
        if (product == null)
        {
            Console.WriteLine($"Product {productId} not found");
            throw new DirectoryNotFoundException("Product not found");
        }

        _context.Favourites.Add(new Favourite()
        {
            ApplicationUserId = userId,
            ProductId = productId,
            FavouriteDate = DateTime.Now
        });
        await _context.SaveChangesAsync();
        return await GetFavouritesAsync(userId);
    }

    public async Task<List<Favourite>> RemoveFromFavouritesAsync(int productId, string userId)
    {
        Favourite? favourite = await _context.Favourites.FirstOrDefaultAsync(f => f.ApplicationUserId == userId && f.ProductId == productId);
        if (favourite == null)
        {
            Console.WriteLine($"Product {productId} is not a favourite");
            throw new ArgumentException("Product is not a favourite");
        }
        _context.Favourites.Remove(favourite);
        await _context.SaveChangesAsync();
        return await GetFavouritesAsync(userId);
    }
}