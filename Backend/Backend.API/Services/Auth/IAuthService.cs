using Backend.API.Models.DTOs;

namespace Backend.API.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto);
        
        
    }
}