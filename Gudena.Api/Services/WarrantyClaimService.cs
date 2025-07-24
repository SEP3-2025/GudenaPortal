using Gudena.Api.DTOs;
using Gudena.Api.Repositories;
using Gudena.Data.Entities;

namespace Gudena.Api.Services;

public class WarrantyClaimService : IWarrantyClaimService
{
    private readonly IWarrantyClaimRepository _warrantyClaimRepository;

    public WarrantyClaimService(IWarrantyClaimRepository warrantyClaimRepository)
    {
        _warrantyClaimRepository = warrantyClaimRepository;
    }
    
    public async Task<List<WarrantyClaim>> GetAsync(string userId)
    {
        return await _warrantyClaimRepository.GetAsync(userId);
    }

    public async Task<WarrantyClaim> GetByIdAsync(string userId, int warrantyClaimId)
    {
        return await _warrantyClaimRepository.GetByIdAsync(userId, warrantyClaimId);
    }

    public async Task<WarrantyClaim> CreateAsync(WarrantyClaimDto warrantyClaimDto, string userId)
    {
        return await _warrantyClaimRepository.CreateAsync(warrantyClaimDto, userId);
    }
}