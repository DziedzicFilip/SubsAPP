using Backend.API.Models.DTOs;
using Backend.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
namespace Backend.API.Services.Groups
{
    public interface IGroupsService
    {
        Task<IActionResult> CreateGroupAsync(string name, string description);
    }


}