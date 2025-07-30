using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gudena.Api.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _context.Payments
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Payment> AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task DeletePaymentAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Payment?> GetPaymentByIdAndUserIdAsync(int paymentId, string userId)
        {
            return await _context.Payments
                .Include(p => p.Order) // Include related Order to access ApplicationUserId
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == paymentId && p.Order.ApplicationUserId == userId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAndUserIdAsync(int orderId, string userId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.OrderId == orderId && p.Order.ApplicationUserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
        
        public async Task<Payment?> GetUnclaimedPaymentByIdAsync(int id, string userId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == id && p.OrderId == null && p.PayingUserId == userId);
        }
    }
}
