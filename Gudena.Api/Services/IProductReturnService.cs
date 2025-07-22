using Gudena.Data.Entities;
using Gudena.Api.DTOs;

namespace Gudena.Api.Services;

public interface IProductReturnService
{
    public Task<List<ProductReturn>> GetAsync(string userId);
    public Task<ProductReturn> GetByIdAsync(string userId, int productReturnId);
    public Task<ProductReturn> CreateAsync(ProductReturnDto productReturnDto, string userId);
}