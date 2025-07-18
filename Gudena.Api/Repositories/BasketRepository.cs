using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Api.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly AppDbContext _context;
    private readonly IProductRepository _productRepository;

    public BasketRepository(AppDbContext context, IProductRepository productRepository)
    {
        _context = context;
        _productRepository = productRepository;
    }
    
    public async Task<Basket?> RetrieveBasketAsync(string userId, int? basketId)
    {
        if (basketId.HasValue) // Retrieve a specific basket
        {
            return await _context.Baskets.SingleOrDefaultAsync(b => b.Id == basketId.Value);
        }
        else // Retrieve last unordered basket if it exists, otherwise create new basket
        {
            Basket basket = await _context.Baskets.SingleOrDefaultAsync(b => b.ApplicationUserId == userId);
            if (basket == null) // Basket doesn't exist
            {
                basket = new Basket();
                await _context.Baskets.AddAsync(basket);
                await _context.SaveChangesAsync();
                return basket;
            }
            else
                return basket;
        }
    }

    public async Task<Basket> AddProductToBasketAsync(int basketId, int productId, int amount)
    {
        Basket basket = await RetrieveBasketAsync(null, basketId);
        if (basket == null)
            throw new Exception("Basket not found");
        BasketItem? basketItem = basket.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
        if (basketItem == null) // Product isn't in the basket
        {
            Product? product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new Exception("Product not found");
            basket.BasketItems.Add(new BasketItem { Product = product, Amount = amount });
        }
        else // Product is already in the basket, add the specified amount
            basketItem.Amount += amount;
        await _context.SaveChangesAsync();
        return basket;
    }

    public async Task<Basket> UpdateProductAmountAsync(int basketId, int productId, int amount)
    {
        if (amount == 0) // If the amount is 0, remove the basketItem
            return await RemoveProductFromBasketAsync(basketId, productId);
        Basket basket = await RetrieveBasketAsync(null, basketId);
        if (basket == null)
            throw new Exception("Basket not found");
        BasketItem? basketItem = basket.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
        if (basketItem == null)
            throw new Exception("BasketItem not found");
        basketItem.Amount = amount;
        await _context.SaveChangesAsync();
        return basket;
    }

    public async Task<Basket> RemoveProductFromBasketAsync(int basketId, int productId)
    {
        Basket basket = await RetrieveBasketAsync(null, basketId);
        if (basket == null)
            throw new Exception("Basket not found");
        BasketItem? basketItem = basket.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
        if (basketItem == null)
            throw new Exception("BasketItem not found");
        _context.BasketItems.Remove(basketItem);
        await _context.SaveChangesAsync();
        return basket;
    }

    public async Task DestroyBasketAsync(int basketId)
    {
        Basket basket = await RetrieveBasketAsync(null, basketId);
        if (basket == null)
            throw new Exception("Basket not found");
        _context.Remove(basket);
        await _context.SaveChangesAsync();
    }
}