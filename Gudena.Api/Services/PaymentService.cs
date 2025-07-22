using Gudena.Api.Repositories;
using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gudena.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;

        public PaymentService(IPaymentRepository repository)
        {
            _repository = repository;
        }

        public async Task<Payment> ProcessPaymentAsync(Basket basket, decimal amount, string paymentMethod)
        {
            var payment = new Payment
            {
                BasketId = basket.Id,
                Amount = amount,
                PaymentMethod = paymentMethod,
                PaymentStatus = "DummyCompleted",
                TransactionDate = DateTime.UtcNow,
                Basket = basket
            };
            await _repository.AddAsync(payment);
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByBasketIdAsync(int basketId)
            => await _repository.GetPaymentsByBasketIdAsync(basketId);

        public async Task<IEnumerable<Payment>> GetAllAsync() => await _repository.GetAllAsync();
        public async Task<Payment?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
        public async Task AddAsync(Payment payment) => await _repository.AddAsync(payment);
        public async Task UpdateAsync(Payment payment) => await _repository.UpdateAsync(payment);
        public async Task DeleteAsync(int id) => await _repository.DeleteAsync(id);
        public async Task UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment = await _repository.GetByIdAsync(paymentId);
            if (payment != null)
            {
                payment.PaymentStatus = status;
                await _repository.UpdateAsync(payment);
            }
        }
        public async Task<Payment?> GetLatestByBasketIdAsync(int basketId)
            => await _repository.GetLatestByBasketIdAsync(basketId);
    }
}
