using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class BusinessOrderRepository : IBusinessOrderRepository
{
    private readonly AppDbContext _context;

    public BusinessOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BusinessOrderDto>> GetOrdersForBusinessAsync(string businessId)
    {
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.OrderItems.Any(oi => oi.Product.OwnerId == businessId))
            .Select(o => new BusinessOrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems
                    .Where(oi => oi.Product.OwnerId == businessId && oi.Status != "Cancelled")
                    .Select(oi => new BusinessOrderItemDto
                    {
                        ProductId = oi.Product.Id,
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        PricePerUnit = oi.PricePerUnit
                    }).ToList()
            })
            .ToListAsync();

        return orders;
    }
    
    public async Task<bool> UpdateOrderStatusAsync(int orderId, string businessId, string status)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == orderId 
                                      && o.OrderItems.Any(oi => oi.Product.OwnerId == businessId));

        if (order == null)
            throw new BusinessOwnershipException("This order does not belong to your business.");

        if (order.Status == status)
            throw new BusinessInvalidStatusChangeException("The order already has this status.");
        
        // Ensure that the status change is a progression and that cancelled and/or completed orderItems aren't handled
        List<OrderItem> orderItems = order.OrderItems.Where(oi => oi.Product.OwnerId == businessId).ToList();
        if (orderItems.Any(oi => oi.Status is "Completed" or "Cancelled"))
            throw new BusinessInvalidStatusChangeException($"The order is already ${orderItems.First().Status}.");
        if (orderItems.Any(oi => oi.Status == "Shipped") && status != "Completed")
            throw new BusinessInvalidStatusChangeException($"Shipped orders can only be marked as Completed.");

        foreach (OrderItem oi in orderItems)
        {
            oi.Status = status;
        }
        
        // Update order status based on current orderItem status
        if (order.OrderItems.All(oi => oi.Status == status))
            order.Status = status;
        else if (order.OrderItems.Any(oi => oi.Status == "Shipped"))
            order.Status = "PartiallyShipped";
        else if (order.OrderItems.Any(oi => oi.Status == "Processing"))
            order.Status = "Processing";
        
        await _context.SaveChangesAsync();
        return true;
    }
    
}
