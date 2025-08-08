using System.Security.Claims;
using Gudena.Api.DTOs;
using Gudena.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gudena.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountDetailsController : ControllerBase
    {
        private readonly IAccountDetailsService _service;

        public AccountDetailsController(IAccountDetailsService service)
        {
            _service = service;
        }

        [HttpPut("update-address")]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressDto dto)
        {
            var userId = User.FindFirstValue("uid") 
                         ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                await _service.UpdateAddressAsync(userId, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}