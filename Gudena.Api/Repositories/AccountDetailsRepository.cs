using Gudena.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gudena.Data.Repositories
{
    public class AccountDetailsRepository : IAccountDetailsRepository
    {
        private readonly AppDbContext _context;

        public AccountDetailsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccountDetails?> GetByUserIdAsync(string userId)
        {
            return await _context.AccountDetails
                .FirstOrDefaultAsync(a => a.ApplicationUserId == userId);
        }
    }
}