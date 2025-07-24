using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public class ProductReturnService : IProductReturnService
{
    private readonly IProductReturnRepository _productReturnRepository;

    public ProductReturnService(IProductReturnRepository productReturnRepository)
    {
        _productReturnRepository = productReturnRepository;
    }
    
    public async Task<List<ProductReturn>> GetAsync(string userId)
    {
        return await _productReturnRepository.GetAsync(userId);
    }

    public async Task<ProductReturn> GetByIdAsync(string userId, int productReturnId)
    {
        return await _productReturnRepository.GetByIdAsync(userId, productReturnId);
    }

    public async Task<ProductReturn> CreateAsync(ProductReturnDto productReturnDto, string userId)
    {
        return await _productReturnRepository.CreateAsync(productReturnDto, userId);
    }
}