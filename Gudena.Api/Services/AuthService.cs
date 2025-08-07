using Gudena.Api.DTOs;
using Gudena.Api.Repositories.Interfaces;
using Gudena.Api.Services.Interfaces;
using Gudena.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Gudena.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IAuthRepository repository, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _config = config;
            _userManager = userManager;
        }

        public async Task<string> RegisterBuyerAsync(BuyerRegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                UserType = "Buyer"
            };

            var result = await _repository.CreateUserAsync(user, dto.Password);
            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(e => e.Description));

            var details = new AccountDetails
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                City = dto.City,
                Street = dto.Street,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                PhoneNumber = dto.PhoneNumber,
                ApplicationUserId = user.Id
            };

            await _repository.AddAccountDetailsAsync(details);
            return "Registration successful.";
        }

        public async Task<string> RegisterBusinessAsync(BusinessRegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                UserType = "Business"
            };

            var result = await _repository.CreateUserAsync(user, dto.Password);
            if (!result.Succeeded)
                return string.Join(", ", result.Errors.Select(e => e.Description));

            var details = new AccountDetails
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                City = dto.City,
                Street = dto.Street,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                PhoneNumber = dto.PhoneNumber,
                CompanyName = dto.CompanyName,
                ApplicationUserId = user.Id
            };

            await _repository.AddAccountDetailsAsync(details);
            return "Business registration successful.";
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _repository.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return null;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("uid", user.Id),
                new Claim("UserType", user.UserType)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:DurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
