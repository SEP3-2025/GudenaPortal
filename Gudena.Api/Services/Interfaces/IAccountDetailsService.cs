namespace Gudena.Services
{
    public interface IAccountDetailsService
    {
        Task<AccountDetails?> GetAccountDetailsForUserAsync(string userId);
    }
}