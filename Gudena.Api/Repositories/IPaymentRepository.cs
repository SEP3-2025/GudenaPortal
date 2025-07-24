using Gudena.Data.Entities;

namespace Gudena.Api.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetPaymentByIdAsync(int id);
    Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
    Task<Payment> AddPaymentAsync(Payment payment);
    Task<Payment> UpdatePaymentAsync(Payment payment);
    Task DeletePaymentAsync(int id);
}
