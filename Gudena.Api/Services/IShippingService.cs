using System.Threading.Tasks;
using Gudena.Data.Entities;

public interface IShippingService
{
    Task<Shipping> CalculateShippingAsync(Basket basket, string shippingAddress, string deliveryOption);
    Task<string> GetShipmentStatusAsync(int id);
}
