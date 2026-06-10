using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Backend.API.Models;
using Backend.API.Models.Entities;
using Backend.API.Models.DTOs;

namespace Backend.API.Services.Token
{
    public interface ITokenService
    {
         Task<AuthResponseDTO> GenerateToken(User user);
    }
}