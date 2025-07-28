using Gudena.Api.DTOs;
using Gudena.Api.Exceptions;
using Gudena.Data;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class BusinessWarrantyClaimRepository : IBusinessWarrantyClaimRepository
{
    private readonly AppDbContext _context;

    public BusinessWarrantyClaimRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BusinessWarrantyClaimDto>> GetWarrantyClaimsAsync(string businessId)
    {
        return await _context.WarrantyClaims
            .Include(wc => wc.OrderItem)
            .ThenInclude(oi => oi.Product)
            .Where(wc => wc.OrderItem.Product.OwnerId == businessId)
            .Select(wc => new BusinessWarrantyClaimDto
            {
                Id = wc.Id,
                OrderItemId = wc.OrderItemId,
                ProductName = wc.OrderItem.Product.Name,
                Quantity = wc.OrderItem.Quantity,
                Reason = wc.Reason,
                Status = wc.Status
            })
            .ToListAsync();
    }

    public async Task<bool> UpdateWarrantyClaimStatusAsync(int warrantyClaimId, string businessId, string status)
    {
        var claim = await _context.WarrantyClaims
            .Include(wc => wc.OrderItem)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(wc => wc.Id == warrantyClaimId
                                       && wc.OrderItem.Product.OwnerId == businessId);

        if (claim == null)
            throw new BusinessOwnershipException("This warranty claim does not belong to your business.");

        if (claim.Status == status)
            throw new BusinessInvalidStatusChangeException("The warranty claim already has this status.");

        claim.Status = status;
        await _context.SaveChangesAsync();
        return true;
    }
}