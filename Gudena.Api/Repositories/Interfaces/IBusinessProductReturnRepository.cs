using Gudena.Api.DTOs;

namespace Gudena.Api.Repositories;

public interface IBusinessProductReturnRepository
{
    Task<List<BusinessProductReturnDto>> GetProductReturnsAsync(string businessId);
    Task<bool> UpdateProductReturnStatusAsync(int productReturnId, string businessId, string status);
}