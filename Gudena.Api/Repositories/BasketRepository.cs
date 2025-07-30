using System.Text.Json;
using System.Text.Json.Serialization;
using Gudena.Data;
using Gudena.Data.Entities;
using Gudena.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
    
    public async Task<Basket?> RetrieveBasketAsync(string userId, int basketId = -1)
    {
        if (basketId != -1) // Retrieve a specific basket
        {
            return await _context.Baskets
                .Include(b => b.BasketItems)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(b => b.Id == basketId);
        }
        else // Retrieve last unordered basket if it exists, otherwise create new basket
        {
            Basket? basket = await _context.Baskets
                .Include(c => c.BasketItems)
                .ThenInclude(ci => ci.Product)
                .SingleOrDefaultAsync(b => b.ApplicationUserId == userId);
            if (basket == null) // Basket doesn't exist
            {
                basket = new Basket()
                {
                    ApplicationUserId = userId
                };
                await _context.Baskets.AddAsync(basket);
                await _context.SaveChangesAsync();
                return basket;
            }
            else
                return basket;
        }
    }

    public async Task<Basket> AddProductToBasketAsync(string userId, int productId, int amount)
    {
        Basket basket = await RetrieveBasketAsync(userId, -1);
        if (basket == null)
            throw new ResourceNotFoundException("Basket not found");
        BasketItem? basketItem = basket.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
        if (basketItem == null) // Product isn't in the basket
        {
            Product? product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new ResourceNotFoundException("Product not found");
            if (product.Stock < amount)
            {
                Console.WriteLine($"Basket addition for {amount} product {productId} exceeds stock {basketItem.Product.Stock}");
                throw new BasketExceedsStockException($"Basket addition for {amount} product {productId} exceeds stock {basketItem.Product.Stock}");
            }
            basket.BasketItems.Add(new BasketItem { Product = product, Amount = amount });
        }
        else // Product is already in the basket, add the specified amount
        {
            basketItem.Amount += amount;
            if (basketItem.Product.Stock < basketItem.Amount)
            {
                Console.WriteLine($"BasketItem {basketItem.Id} quantity update to {amount} exceeds product {basketItem.ProductId} stock {basketItem.Product.Stock}");
                throw new BasketExceedsStockException($"BasketItem {basketItem.Id} quantity update to {amount} exceeds product {basketItem.ProductId} stock {basketItem.Product.Stock}");
            }
        }
        await _context.SaveChangesAsync();
        return basket;
    }

    public async Task<Basket> UpdateProductAmountAsync(string userId, int productId, int amount)
    {
        if (amount == 0) // If the amount is 0, remove the basketItem
            return await RemoveProductFromBasketAsync(userId, productId);
        Basket basket = await RetrieveBasketAsync(userId, -1);
        if (basket == null)
            throw new ResourceNotFoundException("Basket not found");
        if (basket.BasketItems == null)
            basket.BasketItems = new List<BasketItem>();
        BasketItem? basketItem = basket.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
        if (basketItem == null)
            throw new ResourceNotFoundException("BasketItem not found");
        if (basketItem.Product.Stock < amount)
        {
            Console.WriteLine($"BasketItem {basketItem.Id} quantity update to {amount} exceeds product {basketItem.ProductId} stock {basketItem.Product.Stock}");
            throw new BasketExceedsStockException($"BasketItem {basketItem.Id} quantity update to {amount} exceeds product {basketItem.ProductId} stock {basketItem.Product.Stock}");
        }
        basketItem.Amount = amount;
        await _context.SaveChangesAsync();
        return basket;
    }

    public async Task<Basket> RemoveProductFromBasketAsync(string userId, int productId)
    {
        Basket basket = await RetrieveBasketAsync(userId, -1);
        if (basket == null)
            throw new ResourceNotFoundException("Basket not found");
        BasketItem? basketItem = basket.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
        if (basketItem == null)
            throw new ResourceNotFoundException("BasketItem not found");
        _context.BasketItems.Remove(basketItem);
        await _context.SaveChangesAsync();
        return basket;
    }

    public async Task DestroyBasketAsync(string userId)
    {
        Basket basket = await RetrieveBasketAsync(userId, -1);
        if (basket == null)
            throw new ResourceNotFoundException("Basket not found");
        _context.Remove(basket);
        await _context.SaveChangesAsync();
    }
}