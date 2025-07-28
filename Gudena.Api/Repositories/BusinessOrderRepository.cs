using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Data;

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
                    .Where(oi => oi.Product.OwnerId == businessId)
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

        order.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }
    
}
