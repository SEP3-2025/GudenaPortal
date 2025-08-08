using Gudena.Api.DTOs;

namespace Gudena.Data.Repositories
{
    public interface IAccountDetailsRepository
    {
        Task<AccountDetails?> GetByUserIdAsync(string userId);
        Task UpdateAddressAsync(string userId, AddressDto dto);
    }
}