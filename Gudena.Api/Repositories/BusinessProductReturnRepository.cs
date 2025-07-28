using Gudena.Api.DTOs;
using Gudena.Data;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class BusinessProductReturnRepository : IBusinessProductReturnRepository
{
    private readonly AppDbContext _context;

    public BusinessProductReturnRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BusinessProductReturnDto>> GetProductReturnsAsync(string businessId)
    {
        return await _context.ProductReturns
            .Include(pr => pr.OrderItem)
            .ThenInclude(oi => oi.Product)
            .Where(pr => pr.OrderItem.Product.OwnerId == businessId)
            .Select(pr => new BusinessProductReturnDto
            {
                Id = pr.Id,
                OrderItemId = pr.OrderItemId,
                ProductName = pr.OrderItem.Product.Name,
                Quantity = pr.OrderItem.Quantity,
                Reason = pr.Reason,
                Status = pr.Status
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateProductReturnStatusAsync(int productReturnId, string businessId, string status)
    {
        var productReturn = await _context.ProductReturns
            .Include(pr => pr.OrderItem)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(pr => pr.Id == productReturnId 
                                       && pr.OrderItem.Product.OwnerId == businessId);

        if (productReturn == null)
            return false;

        productReturn.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }
}