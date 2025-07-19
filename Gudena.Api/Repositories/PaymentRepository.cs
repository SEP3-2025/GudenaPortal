using System.Collections.Generic;
using System.Threading.Tasks;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Gudena.Api.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        // NEW: Get payment(s) by basket
        public async Task<IEnumerable<Payment>> GetPaymentsByBasketIdAsync(int basketId)
        {
            return await _context.Payments
                .Where(p => p.BasketId == basketId)
                .ToListAsync();
        }

        public async Task<Payment?> GetLatestByBasketIdAsync(int basketId)
        {
            return await _context.Payments
                .Where(p => p.BasketId == basketId)
                .OrderByDescending(p => p.TransactionDate)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
