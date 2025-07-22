using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task<Order> GetOrderAsync(string userId, int orderId)
    {
        return await _orderRepository.GetOrderAsync(userId, orderId);
    }

    public async Task<ICollection<Order>> GetOrdersAsync(string userId)
    {
        return await _orderRepository.GetOrdersAsync(userId);
    }

    public async Task<Order> CreateOrderAsync(OrderDto orderDto, string userId)
    {
        return await _orderRepository.CreateOrderAsync(orderDto, userId);
    }

    public async Task<Order> UpdateOrderAsync(OrderUpdateDto orderDto, string userId)
    {
        return await _orderRepository.UpdateOrderAsync(orderDto, userId);
    }

    public async Task<Order> CancelOrderAsync(string userId, int orderId)
    {
        return await _orderRepository.CancelOrderAsync(userId, orderId);
    }
}