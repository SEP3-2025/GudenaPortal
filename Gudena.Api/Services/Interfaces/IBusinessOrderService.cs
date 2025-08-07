namespace Gudena.Api.Services.Interfaces;

using Gudena.Api.DTOs;
using Gudena.Data.Entities;

public interface IBusinessOrderService
{
    Task<List<BusinessOrderDto>> GetBusinessOrdersAsync(string businessId);
    Task<Order> GetOrderAsync(string businessId, int orderId);
    Task<bool> UpdateOrderStatusAsync(int orderId, string businessId, string status);

}
