using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Backend.API.Models; 
using Backend.API.Data;   
using Backend.API.Models.Entities; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Backend.API.Services.Auth;
using Backend.API.Services.Groups;
using Backend.API.Services.Token;
using Backend.API.Models.Configuations;


var dir = new DirectoryInfo(AppContext.BaseDirectory);
string? foundEnv = null;
while (dir != null)
{
    var candidate = Path.Combine(dir.FullName, ".env");
    if (File.Exists(candidate))
    {
        foundEnv = candidate;
        break;
    }
    dir = dir.Parent;
}

if (foundEnv != null)
{
    Env.Load(foundEnv);
    Console.WriteLine($"Loaded .env from: {foundEnv}");
}
else
{
    Console.WriteLine("Warning: .env not found in parent folders. Make sure JWT_SECRET is set in environment or appsettings.");
}



var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("Jwt");


var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? builder.Configuration["Jwt:Secret"];
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException("JWT_SECRET is missing.");
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("DB_CONNECTION_STRING is missing.");
}

builder.Services.AddScoped<IGroupsService, GroupService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<IAuthService,AuthService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SubsAPP API",
        Description = "Api for SubsApp created by Swashbuckle.AspNetCore",
        Contact = new OpenApiContact
        {
            Name = "SubsAPP Team",
            Email = ""
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Wpisz token w formacie: Bearer {twoj_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddCors(options => 
{
    options.AddPolicy("DevCors", policy =>
    {
       policy.WithOrigins("http://localhost:5173")
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowAnyHeader()
                .AllowCredentials();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
        .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            //expireTime = TimeSpan.FromMinutes(30),
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("Authentication failed: {Message}", context.Exception?.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                var userName = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
                logger.LogInformation("Token validated for user: {UserName}", userName);
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                logger.LogWarning("Authorization challenge: {Error}", context.ErrorDescription ?? "No token provided");
                return Task.CompletedTask;
            }
        };
   });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SubsAPP API V1");
        options.RoutePrefix = string.Empty; 
    });
}

    var httpsPort = Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT");
    if (!string.IsNullOrWhiteSpace(httpsPort))
    {
        app.UseHttpsRedirection();
    }

app.UseCors("DevCors");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using ( var scope = app.Services.CreateScope() )
{
    var services = scope.ServiceProvider;
    try 
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        Console.WriteLine("Database migrated successfully.");

        await DataSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error occurred while migrating database: {ex.Message}");
    }
}
app.Run();
