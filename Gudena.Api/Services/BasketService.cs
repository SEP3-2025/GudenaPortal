using Gudena.Api.Repositories;

namespace Gudena.Api.Services;

public class BasketService : IBasketService
{
    private IBasketRepository _basketRepository;

    public BasketService(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }
    
    public async Task<Basket?> RetrieveBasketAsync(string userId, int? basketId)
    {
        return await _basketRepository.RetrieveBasketAsync(userId, basketId);
    }

    public async Task<Basket> AddProductToBasketAsync(int basketId, int productId, int amount)
    {
        return await _basketRepository.AddProductToBasketAsync(basketId, productId, amount);
    }

    public async Task<Basket> UpdateProductAmountAsync(int basketId, int productId, int amount)
    {
        return await _basketRepository.UpdateProductAmountAsync(basketId, productId, amount);
    }

    public async Task<Basket> RemoveProductFromBasketAsync(int basketId, int productId)
    {
        return await _basketRepository.RemoveProductFromBasketAsync(basketId, productId);
    }

    public async Task DestroyBasketAsync(int basketId)
    {
        await _basketRepository.DestroyBasketAsync(basketId);
    }
}