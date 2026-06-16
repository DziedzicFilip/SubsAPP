using DotNetEnv;

namespace Backend.API.Extensions;

public static class EnvironmentExtensions
{
    public static void   GetEnvDir()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        string? foundEnv = null;
        while (dir != null)
        {
            var candidate = Path.Combine(dir.FullName, ".env");
            if (File.Exists(candidate)) { foundEnv = candidate; break; }
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
        
    }

   

    public static string GetConnectionString()
    {
        string value = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
        ?? throw new InvalidOperationException("DB_CONNECTION_STRING is missing.");

        return value;
    }

    public static string GetJwtSecret()
    {
        string value = Environment.GetEnvironmentVariable("JWT_SECRET")
        ?? throw new InvalidOperationException("JWT_SECRET is missing.");

        return value;
    }
}