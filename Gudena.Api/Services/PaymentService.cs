using System;
using System.Threading.Tasks;
using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;


{
    public class PaymentService : IPaymentService

    {
        private readonly IPaymentRepository _repository;

        public PaymentService(IPaymentRepository repository)
        {
            _repository = repository;
        }

        // Processes a new payment for an order
        public async Task<Payment> ProcessPaymentAsync(Order order, decimal amount, string paymentMethod)
        {
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = amount,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Completed", // or "Pending", based on your flow
                TransactionDate = DateTime.UtcNow
            };
            await _repository.AddAsync(payment);
            return payment;
        }

        // Retrieves a payment by its ID
        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // (Optional) Retrieve all payments for an order
        public async Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId)
        {
            return await _repository.GetPaymentsByOrderIdAsync(orderId);
        }

        // (Optional) Update payment status
        public async Task UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment = await _repository.GetByIdAsync(paymentId);
            if (payment != null)
            {
                payment.PaymentStatus = status;
                await _repository.UpdateAsync(payment);
            }
        }
    }
}
