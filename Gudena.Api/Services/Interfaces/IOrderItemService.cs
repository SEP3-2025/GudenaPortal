using Gudena.Data.Entities;
using System.Threading.Tasks;

namespace Gudena.Api.Services
{
    public interface IOrderItemService
    {
        Task<OrderItem?> GetOrderItemByIdAsync(int id);
    }
}