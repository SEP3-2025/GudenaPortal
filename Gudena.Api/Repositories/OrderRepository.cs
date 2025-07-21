using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IProductRepository _productRepository;

    public OrderRepository(AppDbContext context, IProductRepository productRepository)
    {
        _context = context;
        _productRepository = productRepository;
    }
    
    public async Task<Order> GetOrderAsync(string userId, int orderId)
    {
        Order? order = await _context.Orders
            .Include(o => o.Payments)
            .Include(o => o.Shipping)
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new FileNotFoundException("Order not found", orderId.ToString()); // TODO: Custom exception
        else if (order.ApplicationUserId != userId)
            throw new UnauthorizedAccessException("Order does not belong to this user");
        return order;
    }

    public async Task<ICollection<Order>> GetOrdersAsync(string userId)
    {
        var orders = await _context.Orders
            .Include(o => o.Payments)
            .Include(o => o.Shipping)
            .Include(o => o.OrderItems)
            .Where(o => o.ApplicationUserId == userId).ToListAsync();
        return orders;
    }

    public async Task<Order> CreateOrderAsync(OrderDto orderDto, ApplicationUser user)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        foreach (var basketItem in orderDto.Basket.BasketItems)
        {
            var product = await _productRepository.GetByIdAsync(basketItem.ProductId);
            OrderItem orderItem = new OrderItem()
            {
                Product = product,
                Quantity = basketItem.Amount,
                PricePerUnit = product.Price
            };
            orderItems.Add(orderItem);
        }
        Order newOrder = new Order
        {
            OrderDate = DateTime.Now,
            Status = "Ordered",
            PaymentMethod = "Test Payment", // TODO: Modify once merged with payment logic
            TotalAmount = orderDto.Total,
            ApplicationUserId = orderDto.UserId,
            //ShippingId = orderDto.ShippingId,
            Shipping = new Shipping() { DeliveryOption = "Test Delivery", ShippingAddress = "Test address", ShippingNumbers = "SHIP192933" },
            // TODO: Add paymentId once merged with payment logic
            OrderItems = orderItems,
            ApplicationUser = user
        };
        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();
        return newOrder;
    }

    public async Task<Order> UpdateOrderAsync(OrderUpdateDto orderDto, string userId)
    {
        Order order = await GetOrderAsync(userId, orderDto.Id);
        order.ShippingId = orderDto.ShippingId;
        foreach (var update in orderDto.OrderItemUpdates)
        {
            var orderItem = order.OrderItems.Single(b => b.Id == update.Id);
            if (orderItem.OrderId != orderDto.Id)
                throw new AccessViolationException($"OrderItem {orderItem.Id} does not belong to order {orderDto.Id}");
            if (update.Quantity == 0) // Remove item from order
                order.OrderItems.Remove(orderItem);
            else
            {
                orderItem.Quantity = update.Quantity;
                orderItem.PricePerUnit = update.PricePerUnit;
            }
        }
        order.TotalAmount = orderDto.Total;
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> CancelOrderAsync(string userId, int orderId)
    {
        Order order = await GetOrderAsync(userId, orderId);
        if (order.Status != "Ordered")
            throw new AccessViolationException($"Order {orderId} cannot be cancelled due to status {order.Status}");
        order.Status = "Canceled";
        await _context.SaveChangesAsync();
        return order;
    }
}