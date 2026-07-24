using Backend.API.Models.Entities;
using Backend.API.Models.DTOs;
using Backend.API.Services.Auth;
using Backend.API.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;


namespace Backend.Tests.ServicesTests
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<ITokenService> _mockTokenService;
        private Mock<UserManager<User>> _mockUserManager;
        private Mock<SignInManager<User>> _mockSignInManager;
        private IAuthService _authService;
        private Mock<ILogger<AuthService>> _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockTokenService = new Mock<ITokenService>();
            _mockUserManager = new Mock<UserManager<User>>(
                Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _mockSignInManager = new Mock<SignInManager<User>>(
                _mockUserManager.Object,
                Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                null, null, null, null);
            _mockLogger = new Mock<ILogger<AuthService>>();
            _authService = new AuthService(
                _mockUserManager.Object,
                _mockSignInManager.Object,
                _mockTokenService.Object,
                _mockLogger.Object);
        }

        [Test]
        public async Task Login_WithValidCredentials_ReturnsAuthResponseDTOWithToken()
        {
           
            var loginDto = new LoginDTO
            {
                Email = "admin@twojadomena.pl",
                Password = "Admin123!"
            };

            var fakeUser = new User { Email = loginDto.Email, PasswordHash = loginDto.Password };
            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync(fakeUser);
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(fakeUser, loginDto.Password, false)).ReturnsAsync(SignInResult.Success);
            _mockTokenService.Setup(ts => ts.GenerateToken(fakeUser)).ReturnsAsync("fake-jwt-token");
            _mockUserManager.Setup(um => um.GetRolesAsync(fakeUser)).ReturnsAsync(new List<string> { "User" });

           
            var result = await _authService.LoginAsync(loginDto);

            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Token, Is.EqualTo("fake-jwt-token"));
            Assert.That(result.Message, Is.EqualTo("Login successful."));
            Assert.That(result.User.Role, Is.Not.Null);
            _mockUserManager.Verify(um => um.FindByEmailAsync(loginDto.Email), Times.Once);
            _mockSignInManager.Verify(sm => sm.CheckPasswordSignInAsync(fakeUser, loginDto.Password, false), Times.Once);
            _mockTokenService.Verify(ts => ts.GenerateToken(fakeUser), Times.Once);
            _mockUserManager.Verify(um => um.GetRolesAsync(fakeUser), Times.Once);

        }

        [TestCase(" ")]
        [TestCase("invalidexample.com")]
        [TestCase("invalid@.com")]
        [TestCase("invalid@com")]
        public async Task Login_WithEmptyAndIncorrectEmail_ReturnsAuthResponseDTOWithErrorMessage(string email)
        {
            
            var loginDto = new LoginDTO
            {
                Email = email,
                Password = "Admin123!"
            };
            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync((User)null);
           
            var result = await _authService.LoginAsync(loginDto);

            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid email or password."));

            _mockSignInManager.Verify(sm => sm.CheckPasswordSignInAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
        }

        [Test]
        public async Task Login_WithEmptyPassword_ReturnsAuthResponseDTOWithErrorMessage()
        {
            
            var loginDto = new LoginDTO
            {
                Email = "correct@example.com",
                Password = " "
            };

            var fakeUser = new User { Email = loginDto.Email, PasswordHash = loginDto.Password };
            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync(fakeUser);
            _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(fakeUser, loginDto.Password, false)).ReturnsAsync(SignInResult.Failed);


            var result = await _authService.LoginAsync(loginDto);
            

            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid email or password."));
            
            _mockUserManager.Verify(um => um.FindByEmailAsync(loginDto.Email), Times.Once);
            _mockSignInManager.Verify(sm => sm.CheckPasswordSignInAsync(fakeUser, loginDto.Password, false), Times.Once);
        }

        [Test]
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

            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid email or password."));
            _mockUserManager.Verify(um => um.FindByEmailAsync(loginDto.Email), Times.Once);
            _mockSignInManager.Verify(sm => sm.CheckPasswordSignInAsync(fakeUser, loginDto.Password, false), Times.Once);
        }

        [Test]
        public async Task Login_WithNonExistentUser_ReturnsAuthResponseDTOWithErrorMessage()
        {
            
            var loginDto = new LoginDTO
            {
                Email = "nonexistent@example.com",
                Password = "password"
            };

            _mockUserManager.Setup(um => um.FindByEmailAsync(loginDto.Email)).ReturnsAsync((User)null);

          
            var result = await _authService.LoginAsync(loginDto);

            
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid email or password."));
        }
    }
}