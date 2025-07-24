using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _paymentRepository.GetPaymentByIdAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _paymentRepository.GetPaymentsByOrderIdAsync(orderId);
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            return await _paymentRepository.AddPaymentAsync(payment);
        }

        public async Task<Payment> UpdatePaymentAsync(Payment payment)
        {
            return await _paymentRepository.UpdatePaymentAsync(payment);
        }
    }
}
