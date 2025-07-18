using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Repositories;

public interface IBasketRepository
{
    Task<Basket?> RetrieveBasketAsync(string userId, int? basketId);
    Task<Basket> AddProductToBasketAsync(int basketId, int productId, int amount);
    Task<Basket> UpdateProductAmountAsync(int basketId, int productId, int amount);
    Task<Basket> RemoveProductFromBasketAsync(int basketId, int productId);
    Task DestroyBasketAsync(int basketId);
}