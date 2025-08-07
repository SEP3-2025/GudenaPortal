using Gudena.Api.DTOs;

namespace Gudena.Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterBuyerAsync(BuyerRegisterDto dto);
        Task<string> RegisterBusinessAsync(BusinessRegisterDto dto);
        Task<string?> LoginAsync(LoginDto dto);
    }
}
