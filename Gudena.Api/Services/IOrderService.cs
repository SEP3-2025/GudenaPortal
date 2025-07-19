using Gudena.Api.DTOs;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public interface IOrderService
{
    public Task<Order> GetOrderAsync(string userId, int orderId);
    public Task<ICollection<Order>> GetOrdersAsync(string userId);
    public Task<Order> CreateOrderAsync(OrderDto orderDto, ApplicationUser user);
    public Task<Order> UpdateOrderAsync(OrderUpdateDto orderDto, string userId);
    public Task<Order> CancelOrderAsync(string userId, int orderId);
}