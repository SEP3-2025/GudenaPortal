using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class WarrantyClaimRepository : IWarrantyClaimRepository
{
    private readonly AppDbContext _context;

    public WarrantyClaimRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<WarrantyClaim>> GetAsync(string userId)
    {
        return await _context.WarrantyClaims
            .Include(wc => wc.Product)
            .Where(wc => wc.OrderItem.Order.ApplicationUserId == userId)
            .ToListAsync();
    }

    public async Task<WarrantyClaim> GetByIdAsync(string userId, int warrantyClaimId)
    {
        return await _context.WarrantyClaims
            .Include(wc => wc.Product)
            .FirstOrDefaultAsync(wc => wc.Id == warrantyClaimId && wc.OrderItem.Order.ApplicationUserId == userId);
    }

    public async Task<WarrantyClaim> CreateAsync(WarrantyClaimDto warrantyClaimDto, string userId)
    {
        OrderItem orderItem = await _context.OrderItems.Include(orderItem => orderItem.Order).FirstOrDefaultAsync(oi => oi.Id == warrantyClaimDto.OrderItemId);
        if (orderItem == null)
        {
            Console.WriteLine($"OrderItem {warrantyClaimDto.OrderItemId} not found");
            throw new ArgumentException();
        }
        if (orderItem.WarrantyClaim != null)
        {
            Console.WriteLine($"A warranty claim already exists for OrderItem {warrantyClaimDto.OrderItemId}");
            throw new AccessViolationException();
        }
        if (orderItem.Order.ApplicationUserId != userId)
        {
            Console.WriteLine($"OrderItem {warrantyClaimDto.OrderItemId} not owned by user {userId}");
            throw new UnauthorizedAccessException();
        }
        WarrantyClaim warrantyClaim = new WarrantyClaim()
        {
            ProductId = orderItem.ProductId,
            OrderItemId = warrantyClaimDto.OrderItemId,
            Reason = warrantyClaimDto.Reason,
            ClaimDate = DateTime.Now,
            Status = "Submitted"
        };
        _context.WarrantyClaims.Add(warrantyClaim);
        await _context.SaveChangesAsync();
        return warrantyClaim;
    }
}