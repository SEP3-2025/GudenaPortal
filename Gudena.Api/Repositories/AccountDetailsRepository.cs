using Gudena.Api.DTOs;
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

        public async Task UpdateAddressAsync(string userId, UpdateAddressDto dto)
        {
            var account = await _context.AccountDetails
                .FirstOrDefaultAsync(a => a.ApplicationUserId == userId);

            if (account == null)
                return;

            if (!string.IsNullOrWhiteSpace(dto.Street))
                account.Street = dto.Street;

            if (!string.IsNullOrWhiteSpace(dto.PostalCode))
                account.PostalCode = dto.PostalCode;

            if (!string.IsNullOrWhiteSpace(dto.City))
                account.City = dto.City;

            if (!string.IsNullOrWhiteSpace(dto.Country))
                account.Country = dto.Country;

            await _context.SaveChangesAsync();
        }
    }
}