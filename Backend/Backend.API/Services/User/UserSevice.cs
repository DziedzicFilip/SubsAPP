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
                       // PasswordHash = password,
                        UserName = email,
                        EmailConfirmed = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                   

                    if (await _userManager.FindByEmailAsync(email) != null)
                    {
                        _logger.LogWarning($"Attempted to create a user with a duplicate email: {email}.");
                        return new BadRequestObjectResult("A user with this email already exists.");
                    }

                    var result = await _userManager.CreateAsync(user, password);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
    _logger.LogWarning("Failed to create user with email: {Email}. Errors: {Errors}", email, errors);
    return new BadRequestObjectResult(errors);
                    }
                    var roleResult = await _userManager.AddToRoleAsync(user, "User");
                    if (!roleResult.Succeeded)
                    {
                        _logger.LogWarning($"Failed to assign role to user with email: {email}");
                        return new BadRequestObjectResult("Failed to assign role to user.");
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
               var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"Attempted to update non-existent user with ID {id}.");
                    return new NotFoundResult();
                }

                user.FirstName = firstName;
                user.LastName = lastName;
                user.Email = email;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Failed to update user with ID: {id}");
                    return new BadRequestObjectResult("Failed to update user.");
                }

                _logger.LogInformation($"User updated: {user.FirstName} {user.LastName} (ID: {user.Id})");
                return new OkResult();

            }

          
            public async Task<IActionResult> GetUserAsync(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"Attempted to retrieve non-existent user with ID {id}.");
                    return new NotFoundResult();
                }

                return new OkObjectResult(user);
            }
            

            public async Task<IActionResult> GetListOfUsersAsync()
            {
                var users = await _userManager.Users.ToListAsync();
                _logger.LogInformation($"Users retrieved: {users.Count} users found.");
                return new OkObjectResult(users);
            }

            public async Task<IActionResult> GetUserByEmailAsync(string email)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"Attempted to retrieve non-existent user with email {email}.");
                    return new NotFoundResult();
                }

                return new OkObjectResult(user);
            }

            public async Task<IActionResult> ChangeUserPasswordAsync(string id, string newPassword, string currentPassword)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning($"Attempted to change password for non-existent user with ID {id}.");
                    return new NotFoundResult();
                }

                var passwordCheck = await _userManager.CheckPasswordAsync(user, currentPassword);
                if (!passwordCheck)
                {
                    _logger.LogWarning($"Incorrect current password provided for user with ID {id}.");
                    return new BadRequestObjectResult("Current password is incorrect.");
                }

                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (!result.Succeeded)
                {
                    _logger.LogWarning($"Failed to change password for user with ID: {id}");
                    return new BadRequestObjectResult("Failed to change password.");
                }

                _logger.LogInformation($"Password changed successfully for user: {user.FirstName} {user.LastName} (ID: {user.Id})");
                return new OkResult();
            }

    }

}