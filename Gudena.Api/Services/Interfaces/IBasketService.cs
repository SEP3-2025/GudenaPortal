using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public interface IBasketService
{
    Task<Basket?> RetrieveBasketAsync(string userId, int basketId);
    Task<Basket> AddProductToBasketAsync(string userId, int productId, int amount);
    Task<Basket> UpdateProductAmountAsync(string userId, int productId, int amount);
    Task<Basket> RemoveProductFromBasketAsync(string userId, int productId);
    Task DestroyBasketAsync(string userId);
}