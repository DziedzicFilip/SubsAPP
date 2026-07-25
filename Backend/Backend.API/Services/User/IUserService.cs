using Microsoft.AspNetCore.Mvc;
namespace Backend.API.Services.UserService
{
    public interface IUserService
    {
        Task<IActionResult> CreateUserAsync(string firstName, string lastName, string email, string password );
        Task<IActionResult> DeleteUserAsync(string id);
        Task<IActionResult> UpdateUserAsync(string id, string firstName, string lastName, string email);
        Task<IActionResult> GetUserAsync(string id);
        Task<IActionResult> GetListOfUsersAsync();
    }
}