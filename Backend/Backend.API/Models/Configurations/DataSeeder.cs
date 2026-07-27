using Backend.API.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Backend.API.Models.Configuations
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                    Console.WriteLine($"Role '{roleName}' created.");
                }
            }

            var seedUsers = new[]
            {
                new { Email = "admin@twojadomena.pl", FirstName = "Admin", LastName = "Test", Role = "Admin" },
                new { Email = "user@twojadomena.pl", FirstName = "User", LastName = "Test", Role = "User" }
            };

            foreach (var seedUser in seedUsers)
            {
                var existingUser = await userManager.FindByEmailAsync(seedUser.Email);
                if (existingUser == null)
                {
                    var user = new User
                    {
                        UserName = seedUser.Email,
                        Email = seedUser.Email,
                        FirstName = seedUser.FirstName,
                        LastName = seedUser.LastName,
                        EmailConfirmed = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    var result = await userManager.CreateAsync(user, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, seedUser.Role);
                        Console.WriteLine($"User '{seedUser.Email}' created with password 'Admin123!'.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create user '{seedUser.Email}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
                else
                {
                    if (!await userManager.IsInRoleAsync(existingUser, seedUser.Role))
                    {
                        await userManager.AddToRoleAsync(existingUser, seedUser.Role);
                    }

                    Console.WriteLine($"User '{seedUser.Email}' already exists.");
                }
            }
        }
    }
}
