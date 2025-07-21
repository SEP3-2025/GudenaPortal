using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gudena.Api.Services
{
    public interface IPaymentService
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task AddAsync(Payment payment);
        Task UpdateAsync(Payment payment);
        Task DeleteAsync(int id);
        Task<IEnumerable<Payment>> GetPaymentsByBasketIdAsync(int basketId); // use this!
        Task<Payment> ProcessPaymentAsync(Basket basket, decimal amount, string paymentMethod);
        Task UpdatePaymentStatusAsync(int paymentId, string status);
        Task<Payment?> GetLatestByBasketIdAsync(int basketId);
    }
}
