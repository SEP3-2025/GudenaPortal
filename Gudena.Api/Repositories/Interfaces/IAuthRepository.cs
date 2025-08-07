using Gudena.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Gudena.Api.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task AddAccountDetailsAsync(AccountDetails details);
    }
}
