using Gudena.Api.DTOs;
using Gudena.Data;
using Gudena.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Gudena.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, AppDbContext context)
    {
        _userManager = userManager;
        _config = config;
        _context = context;
    }

    [HttpPost("register/buyer")]
    public async Task<IActionResult> Register(BuyerRegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            UserType = "Buyer"
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var accountDetails = new AccountDetails
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

        _context.AccountDetails.Add(accountDetails);
        await _context.SaveChangesAsync();

        return Ok("Registration successful.");
    }
    
    [HttpPost("register/business")]
    public async Task<IActionResult> RegisterBusiness(BusinessRegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            UserType = "Business"
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var accountDetails = new AccountDetails
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

        _context.AccountDetails.Add(accountDetails);
        await _context.SaveChangesAsync();

        return Ok("Business registration successful.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized("Invalid credentials.");

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

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }
}
