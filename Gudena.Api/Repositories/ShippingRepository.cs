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

        public async Task<Shipping?> GetShippingByIdAsync(int id)
        {
            return await _context.Shippings
                .Include(s => s.OrderItems)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Shipping> AddShippingAsync(Shipping shipping, List<int> orderItemIds)
        {
            if (orderItemIds.Any())
            {
                var orderItems = await _context.OrderItems
                    .Where(oi => orderItemIds.Contains(oi.Id))
                    .ToListAsync();

                if (shipping.OrderItems == null)
                    shipping.OrderItems = new List<OrderItem>();
                else
                    shipping.OrderItems.Clear();

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

            if (existingShipping == null) throw new Exception("Shipping not found");

            // Update scalar properties
            existingShipping.ShippingAddress = shipping.ShippingAddress;
            existingShipping.DeliveryOption = shipping.DeliveryOption;
            existingShipping.ShippingNumbers = shipping.ShippingNumbers;
            existingShipping.ShippingCost = shipping.ShippingCost;
            existingShipping.ShippingStatus = shipping.ShippingStatus;

            // Update many-to-many relations
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
    }
}
