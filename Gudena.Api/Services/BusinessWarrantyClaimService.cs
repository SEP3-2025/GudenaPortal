using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Api.Services.Interfaces;

namespace Gudena.Api.Services;

public class BusinessWarrantyClaimService : IBusinessWarrantyClaimService
{
    private static readonly HashSet<string> AllowedStatuses = new()
    {
        "Processing",
        "Approved",
        "Rejected",
        "Item Collected",
        "Item Fixing In Progress",
        "Item Sent",
        "Completed"
    };

    private readonly IBusinessWarrantyClaimRepository _repository;

    public BusinessWarrantyClaimService(IBusinessWarrantyClaimRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<BusinessWarrantyClaimDto>> GetWarrantyClaimsAsync(string businessId)
    {
        return await _repository.GetWarrantyClaimsAsync(businessId);
    }

    public async Task<bool> UpdateWarrantyClaimStatusAsync(int warrantyClaimId, string businessId, string status)
    {
        if (!AllowedStatuses.Contains(status))
            throw new ArgumentException($"Invalid status value. Allowed: {string.Join(", ", AllowedStatuses)}");

        return await _repository.UpdateWarrantyClaimStatusAsync(warrantyClaimId, businessId, status);
    }
}
