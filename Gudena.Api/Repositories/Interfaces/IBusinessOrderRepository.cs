using Gudena.Api.DTOs;
using Gudena.Data.Entities;

namespace Gudena.Api.Repositories;

public interface IBusinessOrderRepository
{
    Task<List<BusinessOrderDto>> GetOrdersForBusinessAsync(string businessId);
    Task<bool> UpdateOrderStatusAsync(int orderId, string businessId, string status);
    Task<Order> GetOrderAsync(string businessId, int orderId);
}