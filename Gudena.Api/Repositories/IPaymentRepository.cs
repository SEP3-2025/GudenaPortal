using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gudena.Api.Repositories
{
    public interface IPaymentRepository
{
    Task<IEnumerable<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(int id);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task DeleteAsync(int id);
    Task<IEnumerable<Payment>> GetPaymentsByBasketIdAsync(int basketId);
    Task<Payment?> GetLatestByBasketIdAsync(int basketId);
}

}
