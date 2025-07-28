using Gudena.Api.DTOs;

namespace Gudena.Api.Repositories;

public interface IBusinessOrderRepository
{
    Task<List<BusinessOrderDto>> GetOrdersForBusinessAsync(string businessId);
    Task<bool> UpdateOrderStatusAsync(int orderId, string businessId, string status);
}