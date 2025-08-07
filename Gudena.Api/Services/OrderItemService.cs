using Gudena.Api.Repositories;
using Gudena.Data.Entities;
using System.Threading.Tasks;
using Gudena.Api.Repositories.Interfaces;

namespace Gudena.Api.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repository;

        public OrderItemService(IOrderItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(int id)
        {
            return await _repository.GetOrderItemByIdAsync(id);
        }
    }
}