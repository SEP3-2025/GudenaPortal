using System.ComponentModel.Design;
using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class ProductReturnRepository : IProductReturnRepository
{
    private readonly AppDbContext _context;

    public ProductReturnRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<ProductReturn>> GetAsync(string userId)
    {
        return await _context.ProductReturns
            .Include(pr => pr.Product)
            .Where(pr => pr.OrderItem.Order.ApplicationUserId == userId)
            .ToListAsync();
    }

    public async Task<ProductReturn> GetByIdAsync(string userId, int productReturnId)
    {
        return await _context.ProductReturns
            .Include(pr => pr.Product)
            .FirstOrDefaultAsync(pr => pr.Id == productReturnId && pr.OrderItem.Order.ApplicationUserId == userId);
    }

    public async Task<ProductReturn> CreateAsync(ProductReturnDto productReturnDto, string userId)
    {
        OrderItem orderItem = await _context.OrderItems
            .Include(orderItem => orderItem.ProductReturn).Include(orderItem => orderItem.Order)
            .FirstOrDefaultAsync(oi => oi.Id == productReturnDto.OrderItemId);
        if (orderItem == null)
        {
            Console.WriteLine($"OrderItem {productReturnDto.OrderItemId} not found");
            throw new ArgumentException();
        }

        if (!orderItem.Product.isReturnable)
        {
            Console.WriteLine($"Attempted to return non returnable product {orderItem.ProductId} on orderItem {orderItem.Id}");
            throw new CheckoutException();
        }
        if (orderItem.ProductReturn != null)
        {
            Console.WriteLine($"A return request already exists for OrderItem {productReturnDto.OrderItemId}");
            throw new AccessViolationException();
        }
        if (orderItem.Order.ApplicationUserId != userId)
        {
            Console.WriteLine($"OrderItem {productReturnDto.OrderItemId} not owned by user {userId}");
            throw new UnauthorizedAccessException();
        }
        ProductReturn productReturn = new ProductReturn()
        {
            ProductId = orderItem.ProductId,
            OrderItemId = productReturnDto.OrderItemId,
            Reason = productReturnDto.Reason,
            RequestDate = DateTime.Now,
            Status = "Requested"
        };
        _context.ProductReturns.Add(productReturn);
        await _context.SaveChangesAsync();
        return productReturn;
    }
}