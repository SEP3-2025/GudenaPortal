using System.Security.Claims;
using Gudena.Data.Entities;
using Gudena.Api.DTOs;

namespace Gudena.Api.Repositories;

public interface IOrderRepository
{
    public Task<Order> GetOrderAsync(string userId, int orderId);
    public Task<ICollection<Order>> GetOrdersAsync(string userId);
    public Task<Order> CreateOrderAsync(OrderDto orderDto, string userId);
    public Task<Order> UpdateOrderAsync(OrderUpdateDto orderDto, string userId);
    public Task<Order> CancelOrderAsync(string userId, int orderId);
    public Task<Order?> GetOrderByIdAsync(string userId, int orderId);
}