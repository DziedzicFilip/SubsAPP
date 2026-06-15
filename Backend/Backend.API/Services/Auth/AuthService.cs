using Backend.API.Models.DTOs;
using Backend.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Backend.API.Services.Token;



namespace Backend.API.Services.Auth
{

    public class AuthService : IAuthService
    {

         readonly private UserManager<User> _userManager;
         readonly private SignInManager<User> _signInManager;
         private readonly ITokenService _tokenService;
         private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO request)
        {

             try{

                    var user = await _userManager.FindByEmailAsync(request.Email);

                    if ( user == null )
                    {
                        _logger.LogWarning("Login attempt failed: No user found with email {Email}.", request.Email);
                        return new AuthResponseDTO 
                        {
                            IsSuccess = false,
                            Message = "Invalid email or password."
                        };
                    }
              
            

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password,false);
            

            if (!result.Succeeded)
            {
                _logger.LogWarning("Login attempt failed: Incorrect password for user with email {Email}.", request.Email);
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            var token = await _tokenService.GenerateToken(user);
            _logger.LogInformation("User {Email} logged in successfully.", request.Email);
            var userRole = await _userManager.GetRolesAsync(user);
            
            return new AuthResponseDTO{
                IsSuccess = true,
                Message = "Login successful.",
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                User = new UserResponseDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = userRole.FirstOrDefault()
                   

                }
            };
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during login for email {Email}.", request.Email);
            return new AuthResponseDTO
            {
                IsSuccess = false,
                Message = "An unexpected error occurred. Please try again later."
            };
         } 

    }
}
}