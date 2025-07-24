using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class ShippingRepository : IShippingRepository
{
    private readonly AppDbContext _context;

    public ShippingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Shipping?> GetShippingByIdAsync(int id)
    {
        return await _context.Shippings
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Shipping>> GetShippingsByOrderItemIdAsync(int orderItemId)
    {
        return await _context.Shippings
            .Where(s => s.OrderItemId == orderItemId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Shipping> AddShippingAsync(Shipping shipping)
    {
        await _context.Shippings.AddAsync(shipping);
        await _context.SaveChangesAsync();
        return shipping;
    }

    public async Task<Shipping> UpdateShippingAsync(Shipping shipping)
    {
        _context.Shippings.Update(shipping);
        await _context.SaveChangesAsync();
        return shipping;
    }

    public async Task DeleteShippingAsync(int id)
    {
        var shipping = await _context.Shippings.FindAsync(id);
        if (shipping != null)
        {
            _context.Shippings.Remove(shipping);
            await _context.SaveChangesAsync();
        }
    }
}
