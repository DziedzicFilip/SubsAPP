using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Backend.API.Models.DTOs;
using Backend.API.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;




namespace Backend.API.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<TokenService> _llogger;    

        public TokenService(IConfiguration configuration, UserManager<User> userManager, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _llogger = logger;
        }
        public async Task<string> GenerateToken(User user)
        {
          
            var jwtSettings = _configuration.GetSection("JwtSettings");
            if (!jwtSettings.Exists()) jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = jwtSettings["Secret"] ?? Environment.GetEnvironmentVariable("JWT_SECRET");
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("JWT secret is not configured. Set Jwt:Secret in configuration or JWT_SECRET environment variable.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);
            var userRole = roles.FirstOrDefault() ?? "User";
            _llogger.LogInformation($"Generating token for user {user.UserName} with role {userRole}.");
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(ClaimTypes.Name,user.UserName!),
                new Claim(ClaimTypes.Role, userRole)
            };

           

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        }
    }
