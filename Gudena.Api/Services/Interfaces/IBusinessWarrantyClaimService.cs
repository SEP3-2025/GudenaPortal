using Gudena.Api.DTOs;

namespace Gudena.Api.Services.Interfaces;

public interface IBusinessWarrantyClaimService
{
    Task<List<BusinessWarrantyClaimDto>> GetWarrantyClaimsAsync(string businessId);
    Task<bool> UpdateWarrantyClaimStatusAsync(int warrantyClaimId, string businessId, string status);
}