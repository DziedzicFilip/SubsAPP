using DotNetEnv;
using Backend.API.Extensions;


EnvironmentExtensions.GetEnvDir();

var builder = WebApplication.CreateBuilder(args);
var jwtSecret = EnvironmentExtensions.GetJwtSecret();
var connectionString = EnvironmentExtensions.GetConnectionString();

builder.Services.AddApplicationServices();
builder.Services.AddDatabaseServices(connectionString);
builder.Services.AddJwtAuthentication(builder.Configuration, jwtSecret);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCorsPolicy();

var app = builder.Build();
app.ConfigureMiddleware();
await app.SeedDatabaseAsync();

app.Run();
