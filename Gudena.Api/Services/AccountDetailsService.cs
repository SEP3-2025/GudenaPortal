using Gudena.Data.Entities;
using Gudena.Data.Repositories;

namespace Gudena.Services
{
    public class AccountDetailsService : IAccountDetailsService
    {
        private readonly IAccountDetailsRepository _repository;

        public AccountDetailsService(IAccountDetailsRepository repository)
        {
            _repository = repository;
        }

        public async Task<AccountDetails?> GetAccountDetailsForUserAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }
    }
}