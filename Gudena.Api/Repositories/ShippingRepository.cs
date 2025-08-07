using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gudena.Api.Repositories
{
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
                .Include(s => s.OrderItems)
                .ThenInclude(oi => oi.Order)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Shipping>> GetShippingsByUserIdAsync(string userId)
        {
            return await _context.Shippings
                .Where(s => s.OrderItems.Any(oi => oi.Order.ApplicationUserId == userId))
                .Include(s => s.OrderItems)
                .ThenInclude(oi => oi.Order)
                .ToListAsync();
        }

        public async Task<Shipping> AddShippingAsync(Shipping shipping, List<int> orderItemIds)
        {
            // Make sure OrderItems collection is initialized
            shipping.OrderItems ??= new List<OrderItem>();

            if (orderItemIds.Any())
            {
                var orderItems = await _context.OrderItems
                    .Where(oi => orderItemIds.Contains(oi.Id))
                    .ToListAsync();

                foreach (var oi in orderItems)
                    shipping.OrderItems.Add(oi);
            }

            await _context.Shippings.AddAsync(shipping);
            await _context.SaveChangesAsync();
            return shipping;
        }

        public async Task<Shipping> UpdateShippingAsync(Shipping shipping, List<int> orderItemIds)
        {
            var existingShipping = await _context.Shippings
                .Include(s => s.OrderItems)
                .FirstOrDefaultAsync(s => s.Id == shipping.Id);

            if (existingShipping == null) throw new KeyNotFoundException("Shipping not found");

            existingShipping.City = shipping.City;
            existingShipping.Street = shipping.Street;
            existingShipping.PostalCode = shipping.PostalCode;
            existingShipping.Country = shipping.Country;
            existingShipping.DeliveryOption = shipping.DeliveryOption;
            existingShipping.ShippingNumbers = shipping.ShippingNumbers;
            existingShipping.ShippingCost = shipping.ShippingCost;
            existingShipping.ShippingStatus = shipping.ShippingStatus;

            existingShipping.OrderItems.Clear();
            if (orderItemIds.Any())
            {
                var orderItems = await _context.OrderItems
                    .Where(oi => orderItemIds.Contains(oi.Id))
                    .ToListAsync();

                foreach (var oi in orderItems)
                    existingShipping.OrderItems.Add(oi);
            }

            await _context.SaveChangesAsync();
            return existingShipping;
        }
        
        public async Task<List<Shipping>> RetrievePreviews(string userId)
        {
            return _context.Shippings
                .Where(s => s.ApplicationUserId == userId && s.ShippingStatus == "Preview").ToList();
            
        }

        public async Task CleanUpPreviews(string userId)
        {
            List<Shipping> previews = _context.Shippings
                .Where(s => s.ApplicationUserId == userId && s.ShippingStatus == "Preview").ToList();
            _context.Shippings.RemoveRange(previews);
            await _context.SaveChangesAsync();
        }
    }
}
