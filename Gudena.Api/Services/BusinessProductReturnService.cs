using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Api.Services.Interfaces;

namespace Gudena.Api.Services;

public class BusinessProductReturnService : IBusinessProductReturnService
{
    private static readonly HashSet<string> AllowedStatuses = new()
    {
        "Processing",
        "Approved",
        "Rejected",
        "Item Collected",
        "Completed",
    };

    private readonly IBusinessProductReturnRepository _repository;

    public BusinessProductReturnService(IBusinessProductReturnRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BusinessProductReturnDto>> GetProductReturnsAsync(string businessId)
    {
        return await _repository.GetProductReturnsAsync(businessId);
    }

    public async Task<bool> UpdateProductReturnStatusAsync(int productReturnId, string businessId, string status)
    {
        if (!AllowedStatuses.Contains(status))
            throw new ArgumentException($"Invalid status value. Allowed: {string.Join(", ", AllowedStatuses)}");

        return await _repository.UpdateProductReturnStatusAsync(productReturnId, businessId, status);
    }
}