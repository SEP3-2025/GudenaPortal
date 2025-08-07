using Gudena.Api.Repositories.Interfaces;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Gudena.Api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        public AuthRepository(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public async Task AddAccountDetailsAsync(AccountDetails details)
        {
            _context.AccountDetails.Add(details);
            await _context.SaveChangesAsync();
        }
    }
}
