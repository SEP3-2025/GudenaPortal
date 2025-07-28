using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Api.Services.Interfaces;

namespace Gudena.Api.Services;

public class BusinessOrderService : IBusinessOrderService
{
    private readonly IBusinessOrderRepository _repository;
    
    private static readonly HashSet<string> AllowedStatuses = new()
    {
        "Processing",
        "Shipped",
        "Completed",
        "Cancelled"
    };

    public BusinessOrderService(IBusinessOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BusinessOrderDto>> GetBusinessOrdersAsync(string businessId)
    {
        return await _repository.GetOrdersForBusinessAsync(businessId);
    }
    
    public async Task<bool> UpdateOrderStatusAsync(int orderId, string businessId, string status)
    {
        // Validate status before hitting the repository
        if (!AllowedStatuses.Contains(status))
            throw new ArgumentException($"Invalid status value. Allowed: {string.Join(", ", AllowedStatuses)}");

        return await _repository.UpdateOrderStatusAsync(orderId, businessId, status);
    }
}