using Gudena.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Repositories;

public interface IBasketRepository
{
    Task<Basket?> RetrieveBasketAsync(string userId, int basketId);
    Task<Basket> AddProductToBasketAsync(string userId, int productId, int amount);
    Task<Basket> UpdateProductAmountAsync(string userId, int productId, int amount);
    Task<Basket> RemoveProductFromBasketAsync(string userId, int productId);
    Task DestroyBasketAsync(string userId);
    Task<List<AccountDetails>> GetBusinessDetailsForBasketAsync(string userId);
}