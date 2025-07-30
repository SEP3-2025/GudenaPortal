using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    private readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;
    private readonly IShippingRepository _shippingRepository;
    private readonly IPaymentRepository _paymentRepository;

    public OrderRepository(AppDbContext context, IProductRepository productRepository, IBasketRepository basketRepository, IShippingRepository shippingRepository, IPaymentRepository paymentRepository)
    {
        _context = context;
        _productRepository = productRepository;
        _basketRepository = basketRepository;
        _shippingRepository = shippingRepository;
        _paymentRepository = paymentRepository;
    }

    public async Task<Order> GetOrderAsync(string userId, int orderId)
    {
        Order? order = await _context.Orders
            .Include(o => o.Payments)
            .Include(o => o.Shippings)
            .ThenInclude(s => s.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        if (order == null)
            throw new ResourceNotFoundException($"Order not found {orderId}");
        else if (order.ApplicationUserId != userId && !order.OrderItems.Any(oi => oi.Product.OwnerId == userId))
            throw new UserDoesNotOwnResourceException($"Order {orderId} does not belong to this user {userId}");
        return order;
    }

    public async Task<ICollection<Order>> GetOrdersAsync(string userId)
    {
        var orders = await _context.Orders
            .Include(o => o.Payments)
            .Include(o => o.Shippings)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.ApplicationUserId == userId).ToListAsync();
        return orders;
    }

    public async Task<Order> CreateOrderAsync(OrderDto orderDto, string userId)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        Basket basket = await _basketRepository.RetrieveBasketAsync(userId, -1);
        List<Shipping> shippings = new List<Shipping>();
        Payment payment = await _paymentRepository.GetUnclaimedPaymentByIdAsync(orderDto.PaymentId, userId);
        if (payment == null)
            throw new UnpaidException("Payment not found");
        
        foreach (var basketItem in basket.BasketItems)
        {
            var product = await _productRepository.GetByIdAsync(basketItem.ProductId);
            if (product.Stock < basketItem.Amount) // Attempted to order more products than those in stock
            {
                Console.WriteLine($"Attempted to order {basketItem.Amount} while stock is {product.Stock} for product {basketItem.ProductId}");
                throw new OrderExceedsStockException($"Attempted to order {basketItem.Amount} while stock is {product.Stock} for product {basketItem.ProductId}");
            }

            var shippingDto = orderDto.Shipping.FirstOrDefault(s => s.SellerId == product.OwnerId);
            int? shippingId = shippingDto == null ? null : shippingDto.ShippingId;
            List<Shipping> orderItemShippings = new List<Shipping>();
            if (shippingId != null)
            {
                var ship = await _shippingRepository.GetShippingByIdAsync(shippingId.Value);
                shippings.Add(ship);
                orderItemShippings.Add(ship);
            }
            
            OrderItem orderItem = new OrderItem()
            {
                Product = product,
                Quantity = basketItem.Amount,
                PricePerUnit = product.Price,
                Shippings = orderItemShippings,
                Status = "Ordered"
            };
            orderItems.Add(orderItem);
            product.Stock -= basketItem.Amount;
        }
        Order newOrder = new Order
        {
            OrderDate = DateTime.Now,
            Status = "Ordered",
            PaymentMethod = payment.PaymentMethod,
            TotalAmount = orderDto.Total,
            ApplicationUserId = userId,
            Shippings = shippings,
            Payments = new List<Payment>() { payment },
            OrderItems = orderItems
        };
        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();
        await _basketRepository.DestroyBasketAsync(basket.Id); // Destroy basket
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
                throw new UserDoesNotOwnResourceException($"OrderItem {orderItem.Id} does not belong to order {orderDto.Id}");
            if (update.Quantity == 0) // Remove item from order
            {
                order.OrderItems.Remove(orderItem);
                orderItem.Product.Stock += orderItem.Quantity; // Return product to Stock
            }
            else
            {
                orderItem.Quantity = update.Quantity;
                if (orderItem.Product.Stock < update.Quantity - orderItem.Quantity)
                {
                    Console.WriteLine($"Attempted to update order {order.Id} with {update.Quantity} product {update.ProductId} while stock is {orderItem.Product.Stock}");
                    throw new OrderExceedsStockException($"Attempted to update order {order.Id} with {update.Quantity} product {update.ProductId} while stock is {orderItem.Product.Stock}");
                }
                orderItem.Product.Stock += orderItem.Quantity - update.Quantity; // Update product Stock
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
            throw new IncancellableOrderException($"Order {orderId} cannot be cancelled due to status {order.Status}");
        order.Status = "Canceled";
        foreach (OrderItem orderItem in order.OrderItems)
        {
            orderItem.Product.Stock += orderItem.Quantity; // Return product to Stock
        }
        await _context.SaveChangesAsync();
        return order;
    }
}