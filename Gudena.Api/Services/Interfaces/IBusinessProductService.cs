using Gudena.Api.DTOs;

namespace Gudena.Api.Services.Interfaces
{
    public interface IBusinessProductService
    {
        Task<string> CreateProductAsync(string userId, ProductCreateDto dto);
        Task<string> UpdateProductAsync(string userId, int productId, ProductCreateDto dto);
        Task<string> DeleteProductAsync(string userId, int productId);
        Task<object> GetMyProductsAsync(string userId);
    }
}
