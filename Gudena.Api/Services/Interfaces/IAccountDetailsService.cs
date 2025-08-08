using Gudena.Api.DTOs;

namespace Gudena.Services
{
    public interface IAccountDetailsService
    {
        Task<AccountDetails?> GetAccountDetailsForUserAsync(string userId);
        Task UpdateAddressAsync(string userId, UpdateAddressDto dto);
    }
}