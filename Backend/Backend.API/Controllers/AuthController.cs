using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.API.Models.DTOs;
using Backend.API.Models.Entities;
using Backend.API.Services.Auth;
namespace Backend.API.Controllers
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO LoginRequest)
        {
            if (LoginRequest == null)
            {
                return BadRequest("Invalid login request. Empty data provided.");
            }

            var result = await _authService.LoginAsync(LoginRequest);

            if (result ==  null)
            {
                return BadRequest("Invalid login request.");
            }

            if ( result.IsSuccess == false)
            {
                return BadRequest("Invalid login credentials.");
            }

            return Ok(result);

        }
        
    }
}