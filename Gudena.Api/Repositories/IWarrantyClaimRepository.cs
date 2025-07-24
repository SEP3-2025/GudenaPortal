using Gudena.Api.DTOs;
using Gudena.Data.Entities;

namespace Gudena.Api.Repositories;

public interface IWarrantyClaimRepository
{
    public Task<List<WarrantyClaim>> GetAsync(string userId);
    public Task<WarrantyClaim> GetByIdAsync(string userId, int warrantyClaimId);
    public Task<WarrantyClaim> CreateAsync(WarrantyClaimDto warrantyClaimDto, string userId);
}