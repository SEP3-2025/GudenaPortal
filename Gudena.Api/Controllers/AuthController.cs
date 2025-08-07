using Gudena.Api.DTOs;
using Gudena.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register/buyer")]
        public async Task<IActionResult> RegisterBuyer(BuyerRegisterDto dto)
        {
            var result = await _authService.RegisterBuyerAsync(dto);
            if (!result.Contains("successful"))
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("register/business")]
        public async Task<IActionResult> RegisterBusiness(BusinessRegisterDto dto)
        {
            var result = await _authService.RegisterBusinessAsync(dto);
            if (!result.Contains("successful"))
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);
            if (token == null)
                return Unauthorized("Invalid credentials.");

            return Ok(new { token });
        }
    }
}