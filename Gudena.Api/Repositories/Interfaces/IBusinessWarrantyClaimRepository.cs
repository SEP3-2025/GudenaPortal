using Gudena.Api.DTOs;

namespace Gudena.Api.Repositories;

public interface IBusinessWarrantyClaimRepository
{
    Task<List<BusinessWarrantyClaimDto>> GetWarrantyClaimsAsync(string businessId);
    Task<bool> UpdateWarrantyClaimStatusAsync(int warrantyClaimId, string businessId, string status);
}