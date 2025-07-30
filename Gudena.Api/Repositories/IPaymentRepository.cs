using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gudena.Api.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
        Task<Payment> AddPaymentAsync(Payment payment);
        Task<Payment> UpdatePaymentAsync(Payment payment);
        Task DeletePaymentAsync(int id);
        Task<Payment?> GetPaymentByIdAndUserIdAsync(int paymentId, string userId);
        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAndUserIdAsync(int orderId, string userId);
        Task<Payment?> GetUnclaimedPaymentByIdAsync(int id, string userId);

    }
}
