using Gudena.Api.DTOs;
using Gudena.Data.Entities;

namespace Gudena.Api.Repositories;

public interface IProductReturnRepository
{
    public Task<List<ProductReturn>> GetAsync(string userId);
    public Task<ProductReturn> GetByIdAsync(string userId, int productReturnId);
    public Task<ProductReturn> CreateAsync(ProductReturnDto productReturnDto, string userId);
}