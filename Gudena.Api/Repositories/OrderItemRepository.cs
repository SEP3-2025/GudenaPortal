using Gudena.Api.Repositories.Interfaces;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class OrderItemRepository : IOrderItemRepository
{
    private readonly AppDbContext _context;

    public OrderItemRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
    {
        return await _context.OrderItems
            .Include(oi => oi.Product) // ensure product is loaded
            .FirstOrDefaultAsync(oi => oi.Id == id);
    }
}