using Backend.API.Models.Entities;
using Backend.API.Models.DTOs;
using Backend.API.Services.Auth;
using Backend.API.Services.Token;
using Moq;
using Xunit;


namespace Backend.Tests.ServicesTests
{
     public class AuthServiceTests
     {   
            private readonly Mock<ITokenService> _mockTokenService;
            private readonly Mock<IAuthService> _mockAuthService;
            private readonly IAuthService _authService;

            public AuthServiceTests()
            {
                _mockTokenService = new Mock<ITokenService>();
                _mockAuthService = new Mock<IAuthService>();
                _authService = _mockAuthService.Object;
            }


         [Fact]
         public async Task Login_WithValiCredentials_ReturnsAuthResponseDTOWithToken()
         {
            
            var loginDto = new LoginDTO
            {
                Email = "admin@twojadomena.pl",
                Password = "Admin123!"
            };

            var expectedToken = "jwt-token-example";

            _mockAuthService
                .Setup(svc => svc.LoginAsync(It.IsAny<LoginDTO>()))
                .ReturnsAsync(new AuthResponseDTO { IsSuccess = true, Token = expectedToken });

            var result = await _authService.LoginAsync(loginDto);

            
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedToken, result.Token);
            

         }

    }


}