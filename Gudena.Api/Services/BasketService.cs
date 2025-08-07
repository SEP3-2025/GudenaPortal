using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public class BasketService : IBasketService
{
    private IBasketRepository _basketRepository;

    public BasketService(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    
    public async Task<Basket?> RetrieveBasketAsync(string userId, int basketId)
    {
        return await _basketRepository.RetrieveBasketAsync(userId, basketId);
    }

    public async Task<Basket> AddProductToBasketAsync(string userId, int productId, int amount)
    {
        return await _basketRepository.AddProductToBasketAsync(userId, productId, amount);
    }

    public async Task<Basket> UpdateProductAmountAsync(string userId, int productId, int amount)
    {
        return await _basketRepository.UpdateProductAmountAsync(userId, productId, amount);
    }

    public async Task<Basket> RemoveProductFromBasketAsync(string userId, int productId)
    {
        return await _basketRepository.RemoveProductFromBasketAsync(userId, productId);
    }

    public async Task DestroyBasketAsync(string userId)
    {
        await _basketRepository.DestroyBasketAsync(userId);
    }

    public async Task<List<AccountDetails>> GetBusinessDetailsForBasketAsync(string userId)
    {
        return await _basketRepository.GetBusinessDetailsForBasketAsync(userId);
    }
}