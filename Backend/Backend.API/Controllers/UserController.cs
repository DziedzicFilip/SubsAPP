using Microsoft.AspNetCore.Mvc;
using Backend.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Backend.API.Services.UserService;
namespace Backend.API.Controllers
{
    
     [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("User")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO user)
        {
            return await _userService.CreateUserAsync(user.FirstName, user.LastName, user.Email, user.Password);
        }

        
        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            return await _userService.GetUserAsync(id);
        }


        [HttpGet("Users")]
        public async Task<IActionResult> GetListOfUsers()
        {
            return await _userService.GetListOfUsersAsync();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("User/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            return await _userService.DeleteUserAsync(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("User/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDTO user)
        {
            return await _userService.UpdateUserAsync(id, user.FirstName, user.LastName, user.Email);
        }

        [HttpGet("UserByEmail/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            return await _userService.GetUserByEmailAsync(email);
        }

        [HttpPatch("User/{id}/ChangePassword")]
        public async Task<IActionResult> ChangeUserPassword(string id, [FromBody] ChangePasswordDTO changePasswordDTO)
        {
            return await _userService.ChangeUserPasswordAsync(id, changePasswordDTO.NewPassword, changePasswordDTO.CurrentPassword);
        }



    }


}