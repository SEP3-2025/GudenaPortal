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

            var shippingDto = orderDto.Shipping.FirstOrDefault(s => s.BusinessId == product.OwnerId);
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
        await _basketRepository.DestroyBasketAsync(userId); // Destroy basket
        return newOrder;
    }

    public async Task<Order> UpdateOrderAsync(OrderUpdateDto orderDto, string userId)
    {
        Order order = await GetOrderAsync(userId, orderDto.Id);
        decimal refundedAmount = 0;
        order.ShippingId = orderDto.ShippingId;
        foreach (var update in orderDto.OrderItemUpdates)
        {
            var orderItem = order.OrderItems.Single(b => b.Id == update.Id);
            if (orderItem.OrderId != orderDto.Id)
                throw new UserDoesNotOwnResourceException($"OrderItem {orderItem.Id} does not belong to order {orderDto.Id}");
            if (orderItem.Status != "Ordered")
                throw new CannotModifyOrderException($"OrderItem {orderItem.Id} from order {orderDto.Id} cannot be modified due to status {orderItem.Status}");
            if (update.Quantity == 0) // Remove item from order
            {
                orderItem.Status = "Cancelled";
                orderItem.Product.Stock += orderItem.Quantity; // Return product to Stock
                decimal refundAmount = orderItem.PricePerUnit * orderItem.Quantity;
                // If there is no products left from this merchant refund and cancel shipping
                if (!order.OrderItems.Any(oi => oi.Product.OwnerId == orderItem.Product.OwnerId && oi.Status != "Cancelled"))
                {
                    Shipping shipping =
                        orderItem.Shippings.FirstOrDefault(s => s.OrderItems.Any(oi => oi.Id == orderItem.Id));
                    if (shipping != null)
                    {
                        refundAmount += shipping.ShippingCost;
                        shipping.ShippingStatus = "Cancelled";
                    }
                }
                Payment refund = new Payment()
                {
                    OrderId = order.Id,
                    Amount = refundAmount,
                    PaymentMethod = "Refund",
                    PayingUserId = orderItem.Product.OwnerId,
                    TransactionDate = DateTime.Now,
                    PaymentStatus = "Completed",
                    TransactionId = $"GudenaPay-{Guid.NewGuid()}"
                };
                order.Payments.Add(refund);
                refundedAmount += refundAmount;
            }
            else
            {
                orderItem.Status = "Cancelled";
                OrderItem newOrderItem = new OrderItem()
                {
                    OrderId = order.Id,
                    PricePerUnit = update.PricePerUnit,
                    ProductId = update.ProductId,
                    Quantity = update.Quantity,
                    Status = "Ordered"
                };
                if (orderItem.Product.Stock < update.Quantity - orderItem.Quantity)
                {
                    Console.WriteLine($"Attempted to update order {order.Id} with {update.Quantity} product {update.ProductId} while stock is {orderItem.Product.Stock}");
                    throw new OrderExceedsStockException($"Attempted to update order {order.Id} with {update.Quantity} product {update.ProductId} while stock is {orderItem.Product.Stock}");
                }
                orderItem.Product.Stock += orderItem.Quantity - update.Quantity; // Update product Stock
                decimal oldPrice = orderItem.PricePerUnit * orderItem.Quantity;
                decimal newPrice = update.PricePerUnit * update.Quantity;
                // If the paid price is higher issue a refund
                if (oldPrice > newPrice)
                {
                    Payment refund = new Payment()
                    {
                        OrderId = order.Id,
                        Amount = oldPrice - newPrice,
                        PaymentMethod = "Refund",
                        PayingUserId = orderItem.Product.OwnerId,
                        TransactionDate = DateTime.Now,
                        PaymentStatus = "Completed",
                        TransactionId = $"GudenaPay-{Guid.NewGuid()}"
                    };
                    order.Payments.Add(refund);
                    refundedAmount += refund.Amount;
                }
                order.OrderItems.Add(newOrderItem);
            }
        }

        if (order.TotalAmount < orderDto.Total) // If the total price increased, check payment validity
        {
            Payment payment = await _paymentRepository.GetUnclaimedPaymentByIdAsync(orderDto.PaymentId, userId);
            if (payment == null)
                throw new UnpaidException("Payment not found");
            if (order.TotalAmount + payment.Amount - refundedAmount != orderDto.Total)
                throw new UnpaidException($"Payment {payment.Id} is unfit with the order modification, required payment {orderDto.Total - order.TotalAmount + refundedAmount} actual payment {payment.Amount}");
            order.Payments.Add(payment);
        }
        order.TotalAmount = orderDto.Total;
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> CancelOrderAsync(string userId, int orderId)
    {
        Order order = await GetOrderAsync(userId, orderId);
        if (order.Status != "Ordered" || order.OrderItems.Any(oi => oi.Status != "Ordered"))
            throw new IncancellableOrderException($"Order {orderId} cannot be cancelled due to status {order.Status}");
        order.Status = "Canceled";
        foreach (OrderItem orderItem in order.OrderItems)
        {
            orderItem.Product.Stock += orderItem.Quantity; // Return product to Stock
            orderItem.Status = "Cancelled"; // Cancel OrderItem
            Payment refund = new Payment() // Issue a refund
            {
                OrderId = order.Id,
                Amount = orderItem.PricePerUnit * orderItem.Quantity,
                PaymentMethod = "Refund",
                PayingUserId = orderItem.Product.OwnerId,
                TransactionDate = DateTime.Now,
                PaymentStatus = "Completed",
                TransactionId = $"GudenaPay-{Guid.NewGuid()}"
            };
            order.Payments.Add(refund);
        }
        await _context.SaveChangesAsync();
        return order;
    }
}