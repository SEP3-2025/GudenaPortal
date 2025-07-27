namespace Gudena.Api.Services.Interfaces;

using Gudena.Api.DTOs;

public interface IBusinessOrderService
{
    Task<List<BusinessOrderDto>> GetBusinessOrdersAsync(string businessId);
    Task<bool> UpdateOrderStatusAsync(int orderId, string businessId, string status);

}
