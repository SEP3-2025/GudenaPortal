using System.Collections.Generic;
using System.Threading.Tasks;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        private readonly AppDbContext _context;

        public ShippingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipping>> GetAllAsync()
        {
            return await _context.Shippings.ToListAsync();
        }

        public async Task<Shipping?> GetByIdAsync(int id)
        {
            return await _context.Shippings.FindAsync(id);
        }

        public async Task AddAsync(Shipping shipping)
        {
            _context.Shippings.Add(shipping);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Shipping shipping)
        {
            _context.Shippings.Update(shipping);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);
            if (shipping != null)
            {
                _context.Shippings.Remove(shipping);
                await _context.SaveChangesAsync();
            }
        }
    }
}