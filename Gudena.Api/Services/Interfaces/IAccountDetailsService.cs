using Gudena.Api.DTOs;

namespace Gudena.Services
{
    public interface IAccountDetailsService
    {
        Task<AccountDetails?> GetAccountDetailsForUserAsync(string userId);
        Task UpdateAddressAsync(string userId, AddressDto dto);
        Task<AddressDto?> GetAddressAsync(string userId);   
    }
}