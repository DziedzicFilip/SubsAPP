using Backend.API.Data;
using Microsoft.AspNetCore.Mvc;
using Backend.API.Models.Entities;
using  Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Backend.API.Services.UserService
{


    public class UserService : IUserService
    {
            private readonly ILogger<UserService> _logger;
            private readonly UserManager<User> _userManager;



            public UserService(ILogger<UserService> logger, UserManager<User> userManager)
            {
                _logger = logger;
                _userManager = userManager;
            }

            public async Task<IActionResult> CreateUserAsync(string firstName, string lastName, string email, string password)
            {
                try
                {
                    var user = new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        PasswordHash = password
                    };

                    if (await _userManager.FindByEmailAsync(email) != null)
                    {
                        _logger.LogWarning($"Attempted to create a user with a duplicate email: {email}.");
                        return new BadRequestObjectResult("A user with this email already exists.");
                    }

                    var result = await _userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        _logger.LogWarning($"Failed to create user with email: {email}");
                        return new BadRequestObjectResult("Failed to create user.");
                    }

                    _logger.LogInformation($"User created: {user.FirstName} {user.LastName} (ID: {user.Id})");
                    return new OkResult();
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogWarning(ex, "DbUpdateException occurred while creating user {UserEmail}.", email);
                    return new BadRequestObjectResult("Cannot create user. Ensure data constraints are met (e.g. unique email).");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred while creating a user.");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            public async Task<IActionResult> DeleteUserAsync(string id)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user == null)
                    {
                        _logger.LogWarning($"Attempted to delete non-existent user with ID {id}.");
                        return new NotFoundResult();
                    }

                  
                    user.IsActive = false;
                    user.UpdatedAt = DateTime.UtcNow;

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        _logger.LogWarning($"Failed to deactivate user with ID: {id}");
                        return new BadRequestObjectResult("Failed to deactivate user.");
                    }

                    _logger.LogInformation($"User deactivated: {user.FirstName} {user.LastName} (ID: {user.Id})");
                    return new OkResult();
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogWarning(ex, "DbUpdateException occurred while deactivating user with ID {UserId}.", id);
                    return new BadRequestObjectResult("Cannot deactivate user. Ensure data constraints are met.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred while deactivating a user.");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            public async Task<IActionResult> UpdateUserAsync(string id, string firstName, string lastName, string email)
            {
               return new BadRequestObjectResult("Not yet implemented");

            }

          
            public async Task<IActionResult> GetUserAsync(string id)
            {
                return new BadRequestObjectResult("Not yet implemented");
            }

            public async Task<IActionResult> GetListOfUsersAsync()
            {
                return new BadRequestObjectResult("Not yet implemented");
            }

    }

}