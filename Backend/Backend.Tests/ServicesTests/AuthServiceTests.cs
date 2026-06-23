using Backend.API.Models.Entities;
using Backend.API.Models.DTOs;
using Backend.API.Services.Auth;
using Backend.API.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using Xunit;


namespace Backend.Tests.ServicesTests
{
     public class AuthServiceTests
     {   
            private readonly Mock<ITokenService> _mockTokenService;
            private readonly Mock<UserManager<User>> _mockUserManager;
            private readonly Mock<SignInManager<User>> _mockSignInManager;
            private readonly IAuthService _authService;
            private readonly Mock<ILogger<AuthService>> _mockLogger;

            public AuthServiceTests()
            {
                _mockTokenService = new Mock<ITokenService>();
                _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
                _mockSignInManager = new Mock<SignInManager<User>>(_mockUserManager.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null, null, null, null);
                _mockLogger = new Mock<ILogger<AuthService>>();
                _authService = new AuthService(_mockUserManager.Object, _mockSignInManager.Object,_mockTokenService.Object, _mockLogger.Object);
            }


         [Fact]
         public async Task Login_WithValiCredentials_ReturnsAuthResponseDTOWithToken()
         {
            
            var loginDto = new LoginDTO
            {
                Email = "admin@twojadomena.pl",
                Password = "Admin123!"
            };

            var fakeUser = new User {Email = loginDto.Email, PasswordHash = loginDto.Password};
            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync(fakeUser);
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(fakeUser, loginDto.Password, false)).ReturnsAsync(SignInResult.Success);
            _mockTokenService.Setup(ts => ts.GenerateToken(fakeUser)).ReturnsAsync("fake-jwt-token");
            _mockUserManager.Setup(um => um.GetRolesAsync(fakeUser)).ReturnsAsync(new List<string> { "User" });
              
            var result = await _authService.LoginAsync(loginDto);

            
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("fake-jwt-token", result.Token);
            
            

         }

         [Fact]
         public async Task Login_WithEmptyEmail_ReturnsAuthResponseDTOWithErrorMessage()
            {
                var loginDto = new LoginDTO
                {
                    Email = " ",
                    Password = "Admin123!"
                };

                var result = await _authService.LoginAsync(loginDto);
                Assert.NotNull(result);
                Assert.False(result.IsSuccess);
                Assert.Equal("Invalid email or password.", result.Message);
        

            }

        [Fact]
        public async Task Login_WithEmptyPassword_ReturnsAuthResponseDTOWithErrorMessage()
        {
            var loginDto = new LoginDTO
            {
                Email = "correct@example.com",
                Password = " "
            };

            
            var result = await _authService.LoginAsync(loginDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid email or password.", result.Message);

        }

        [Fact]
        public async Task Login_WithIncorrectPassword_ReturnsAuthResponseDTOWithErrorMessage()
        {
            var loginDto = new LoginDTO
            {
                Email = "incorrect@example.com",
                Password = "wrongpassword"
            };

            var fakeUser = new User { Email = loginDto.Email, PasswordHash = "correctpassword" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync(fakeUser);
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(fakeUser, loginDto.Password, false)).ReturnsAsync(SignInResult.Failed);
            
            var result = await _authService.LoginAsync(loginDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid email or password.", result.Message);


        }

        [Fact]
        public async Task Login_WithNonExistentUser_ReturnsAuthResponseDTOWithErrorMessage()
        {
            var loginDto = new LoginDTO
            {
                Email = "nonexistent@example.com",
                Password = "password"
            };

            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync((User)null);

            var result = await _authService.LoginAsync(loginDto);

            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal("Invalid email or password.", result.Message);
        }    


        
    }

}