using Gudena.Data.Entities;

namespace Gudena.Api.Repositories.Interfaces;

public interface IOrderItemRepository
{
    Task<OrderItem?> GetOrderItemByIdAsync(int id);
}