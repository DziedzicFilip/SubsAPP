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
         private readonly TokenService _tokenService;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO request)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);

            if ( user == null )
            {
                return new AuthResponseDTO 
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password,false);
            

            if (!result.Succeeded)
            {
                return new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid email or password."
                };
            }

            var token = await _tokenService.GenerateToken(user);

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
                   

                }
            };
        }

    }
}