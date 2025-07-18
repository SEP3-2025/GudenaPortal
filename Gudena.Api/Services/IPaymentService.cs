using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gudena.Api.Services
{
    public interface IPaymentService
    {
        Task<Payment> ProcessPaymentAsync(Order order, decimal amount, string paymentMethod);
        Task<Payment?> GetPaymentByIdAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
        Task UpdatePaymentStatusAsync(int paymentId, string status);
    }
}
