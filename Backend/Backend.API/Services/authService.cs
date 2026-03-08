namespace SubsAPP.Backend.API.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly TokenService _tokenService;

         public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            AppDbContext context,
            IEmailService emailService,
            TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var user = await _context.Users.FirstORDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
                return null;
            
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return null;

            var token = _tokenService.CreateToken(user, await _userManager.GetRolesAsync(user));

            return new AuthResponseDTO
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1),
                User = new UserResponseDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                }
            };

        }
    }
}