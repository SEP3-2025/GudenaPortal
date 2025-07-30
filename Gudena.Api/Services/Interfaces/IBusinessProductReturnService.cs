namespace Gudena.Api.Services.Interfaces;

using Gudena.Api.DTOs;

public interface IBusinessProductReturnService
{
    Task<List<BusinessProductReturnDto>> GetProductReturnsAsync(string businessId);
    Task<bool> UpdateProductReturnStatusAsync(int productReturnId, string businessId, string status);
}
