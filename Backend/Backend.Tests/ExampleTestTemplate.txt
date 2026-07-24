using Backend.API.Models.Entities;
using Backend.API.Models.DTOs;
using Backend.API.Services.Auth;
using Moq;
using Xunit;

namespace Backend.Tests.ServicesTests
{
    // Nazwa klasy testowej: [TestawaKlasa]Tests
    public class AuthServiceTemplateTests
    {
        // ============================================================
        // 1. PRZYGOTOWANIE - Pola i fixture setup
        // ============================================================
        
        // Mockuj zależności jako pola prywatne readonly
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<ITokenService> _mockTokenService;
        
        // Rzeczywista klasa do testowania
        private readonly AuthService _authService;

        // Konstruktor: inicjalizacja mocków i instancji testowanej klasy
        public AuthServiceTemplateTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTokenService = new Mock<ITokenService>();
            
            // Utwórz instancję rzeczywistej klasy z mockami
            _authService = new AuthService(_mockUserRepository.Object, _mockTokenService.Object);
        }

        // ============================================================
        // 2. TEST POZYTYWNY - Przebieg normalny (Happy Path)
        // ============================================================
        
        // Naming: MethodName_StateUnderTest_ExpectedBehavior
        [Fact(Skip = "Template example - not a real test")]
        public async Task Login_WithValidCredentials_ReturnsAuthResponseDTOWithToken()
        {
            // ARRANGE - Przygotuj dane i mocki
            var loginDto = new LoginDTO 
            { 
                Email = "test@example.com", 
                Password = "Password123!" 
            };
            
            var user = new User 
            { 
                Id = "user-123", 
                Email = "test@example.com",
                FirstName = "Jan",
                LastName = "Kowalski"
            };
            
            var expectedToken = "jwt-token-here";
            
            // Skonfiguruj mocki - kiedy zostaną wywołane, zwróć co?
            _mockUserRepository
                .Setup(repo => repo.GetUserByEmailAsync(loginDto.Email))
                .ReturnsAsync(user);
            
            _mockTokenService
                .Setup(svc => svc.GenerateToken(user))
                .ReturnsAsync(expectedToken);

            // ACT - Wykonaj testowaną metodę
            var result = await _authService.Login(loginDto);

            // ASSERT - Sprawdź wynik
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedToken, result.Token);
            Assert.Equal(user.Id, result.User.Id);
            
            // VERIFY - Sprawdź, czy mocki były wywołane prawidłowo
            _mockUserRepository.Verify(repo => repo.GetUserByEmailAsync(loginDto.Email), Times.Once);
            _mockTokenService.Verify(svc => svc.GenerateToken(user), Times.Once);
        }

        // ============================================================
        // 3. TEST NEGATYWNY - Błędne dane wejściowe
        // ============================================================
        
        [Fact(Skip = "Template example - not a real test")]
        public async Task Login_WithInvalidEmail_ThrowsArgumentException()
        {
            // ARRANGE
            var invalidLoginDto = new LoginDTO 
            { 
                Email = "invalid-email", // Błędny format email
                Password = "Password123!" 
            };
            
            _mockUserRepository
                .Setup(repo => repo.GetUserByEmailAsync(invalidLoginDto.Email))
                .ReturnsAsync((User)null);

            // ACT & ASSERT - Użyj Assert.ThrowsAsync dla metod async
            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => _authService.Login(invalidLoginDto)
            );
            
            Assert.Contains("Invalid email", exception.Message);
        }

        // ============================================================
        // 4. TEST Z PARAMETRAMI - Wiele przypadków w jednym teście
        // ============================================================
        
        // Użyj [Theory] zamiast [Fact] i dodaj [InlineData] dla każdego wariantu
        [Theory]
        [InlineData("short", false)]           // Hasło za krótkie
        [InlineData("ValidPassword123!", true)] // Prawidłowe hasło
        [InlineData("", false)]                // Puste hasło
        public async Task ValidatePassword_WithVariousInputs_ReturnsExpectedResult(string password, bool expectedValid)
        {
            // ARRANGE
            var user = new User { Id = "user-1", Email = "test@example.com" };

            // ACT
            var result = _authService.ValidatePassword(password);

            // ASSERT
            Assert.Equal(expectedValid, result);
        }

        // ============================================================
        // 5. TEST INTERAKCJI Z MOCKAMI - Weryfikacja wywołań
        // ============================================================
        
        [Fact(Skip = "Template example - not a real test")]
        public async Task Login_VerifiesMockCalledCorrectly()
        {
            // ARRANGE
            var loginDto = new LoginDTO { Email = "user@test.com", Password = "Pass123!" };
            var user = new User { Id = "id-1", Email = "user@test.com" };
            
            _mockUserRepository
                .Setup(repo => repo.GetUserByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            
            _mockTokenService
                .Setup(svc => svc.GenerateToken(It.IsAny<User>()))
                .ReturnsAsync("token");

            // ACT
            await _authService.Login(loginDto);

            // ASSERT - Weryfikuj dokładnie, z jakimi argumentami został wywołany mock
            _mockUserRepository.Verify(
                repo => repo.GetUserByEmailAsync(loginDto.Email), 
                Times.Once,
                "GetUserByEmailAsync should be called once with correct email"
            );
            
            // Weryfikuj że inna metoda NIE została wywołana
            _mockTokenService.Verify(
                svc => svc.RevokeToken(It.IsAny<string>()), 
                Times.Never,
                "RevokeToken should not be called"
            );
        }

        // ============================================================
        // 6. TEST Z WIELOMA ASERCJAMI
        // ============================================================
        
        [Fact(Skip = "Template example - not a real test")]
        public async Task Login_ReturnsCompleteAuthResponse()
        {
            // ARRANGE
            var loginDto = new LoginDTO { Email = "test@example.com", Password = "Pass123!" };
            var user = new User 
            { 
                Id = "user-123",
                Email = "test@example.com",
                FirstName = "Jan",
                LastName = "Kowalski"
            };
            
            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            _mockTokenService.Setup(t => t.GenerateToken(It.IsAny<User>())).ReturnsAsync("jwt-token");

            // ACT
            var result = await _authService.Login(loginDto);

            // ASSERT - Sprawdzaj różne aspekty wyniku
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Token);
            Assert.NotNull(result.User);
            Assert.Equal("test@example.com", result.User.Email);
            Assert.Equal("Jan", result.User.FirstName);
            Assert.NotEqual(default, result.Expiration);
        }

        // ============================================================
        // 7. BEST PRACTICE - Oddzielne fixturey dla różnych scenariuszy
        // ============================================================
        
        private LoginDTO CreateValidLoginDto() 
            => new LoginDTO { Email = "user@example.com", Password = "ValidPass123!" };

        private User CreateTestUser(string id = "user-123") 
            => new User 
            { 
                Id = id, 
                Email = "user@example.com",
                FirstName = "Test",
                LastName = "User"
            };

        [Fact(Skip = "Template example - not a real test")]
        public async Task Login_UsingHelperMethods_CleanerCode()
        {
            // ARRANGE - Kod jest czysty dzięki metodom pomocniczym
            var loginDto = CreateValidLoginDto();
            var user = CreateTestUser();
            
            _mockUserRepository.Setup(r => r.GetUserByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _mockTokenService.Setup(t => t.GenerateToken(user)).ReturnsAsync("token");

            // ACT
            var result = await _authService.Login(loginDto);

            // ASSERT
            Assert.NotNull(result);
        }
    }
}
