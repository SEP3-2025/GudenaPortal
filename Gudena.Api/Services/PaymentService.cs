using System;
using System.Threading.Tasks;
using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services


{
    public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;

    public PaymentService(IPaymentRepository repository)
    {
        _repository = repository;
    }

    // Processes a new payment for a basket
    public async Task<Payment> ProcessPaymentAsync(Basket basket, decimal amount, string paymentMethod)
    {
        var payment = new Payment
        {
            BasketId = basket.Id,
            Amount = amount,
            PaymentMethod = paymentMethod,
            PaymentStatus = "Completed", // or "Pending", etc.
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

    // Retrieve all payments for a basket
    public async Task<IEnumerable<Payment>> GetPaymentsByBasketIdAsync(int basketId)
    {
        return await _repository.GetPaymentsByBasketIdAsync(basketId);
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
