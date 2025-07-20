using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public interface IBasketService
{
    Task<Basket?> RetrieveBasketAsync(string userId, int basketId);
    Task<Basket> AddProductToBasketAsync(int basketId, int productId, int amount);
    Task<Basket> UpdateProductAmountAsync(int basketId, int productId, int amount);
    Task<Basket> RemoveProductFromBasketAsync(int basketId, int productId);
    Task DestroyBasketAsync(int basketId);
}