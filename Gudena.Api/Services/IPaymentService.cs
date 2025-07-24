using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public interface IPaymentService
{
    Task<Payment?> GetPaymentByIdAsync(int id);
    Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
    Task<Payment> CreatePaymentAsync(Payment payment);
    Task<Payment> UpdatePaymentAsync(Payment payment);
    Task DeletePaymentAsync(int id);
}
