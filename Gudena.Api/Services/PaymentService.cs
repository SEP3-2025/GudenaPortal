using Gudena.Api.Repositories;
using Gudena.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<Payment> RefundPaymentAsync(Payment payment)
        {
            // Change payment status to refunded
            payment.PaymentStatus = "Refunded";

            // Save the update
            return await _paymentRepository.UpdatePaymentAsync(payment);
        }

        public async Task<Payment?> GetPaymentByIdAndUserIdAsync(int paymentId, string userId)
        {
            return await _paymentRepository.GetPaymentByIdAndUserIdAsync(paymentId, userId);
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAndUserIdAsync(int orderId, string userId)
        {
            return await _paymentRepository.GetPaymentsByOrderIdAndUserIdAsync(orderId, userId);
}

    }
}
