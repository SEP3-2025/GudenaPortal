namespace Gudena.Data.Repositories
{
    public interface IAccountDetailsRepository
    {
        Task<AccountDetails?> GetByUserIdAsync(string userId);
    }
}