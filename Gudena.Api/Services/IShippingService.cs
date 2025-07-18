using System.Threading.Tasks;
using Gudena.Data.Entities;

namespace Gudena.Api.Services
{
    public interface IShippingService
    {
        Task<Shipping> CalculateShippingAsync(Order order, string shippingAddress);
        Task<Shipping> CreateShipmentAsync(Order order, string shippingAddress);
        Task<string> GetShipmentStatusAsync(int shippingId);
    }
}