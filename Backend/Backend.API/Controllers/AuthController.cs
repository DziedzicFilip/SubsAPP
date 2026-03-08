

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
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            if (result.Token == null)
                return Unauthorized(new { message = "Nieprawidłowy email lub hasło" });

            return Ok(result);
        }
        
    }
}