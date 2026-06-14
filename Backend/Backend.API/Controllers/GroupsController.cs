using Backend.API.Services.Groups;
using Microsoft.AspNetCore.Mvc;
using Backend.API.Models.DTOs;

namespace Backend.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
            private readonly IGroupsService _groupsService;
            public GroupsController(IGroupsService groupsService)
            {
                _groupsService = groupsService;
            }
            [HttpPost("CreateGroup")]
            public async Task<IActionResult> CreateGroup([FromBody] CreateUpdateGroupDTO group)
            {
                return await _groupsService.CreateGroupAsync(group.Name, group.Description);
            }
            [HttpDelete("DeleteGroup/{id}")]
            public async Task<IActionResult> DeleteGroup(int id)
            {
                return await _groupsService.DeleteGroupAsync(id);
            }
            [HttpGet("GetGroup/{id}")]
            public async Task<IActionResult> GetGroup(int id)
            {
                return await _groupsService.GetGroupAsync(id);
            }
            [HttpPut("UpdateGroup/{id}")]
            public async Task<IActionResult> UpdateGroup(int id, [FromBody] CreateUpdateGroupDTO group)
            {
                return await _groupsService.UpdateGroupAsync(id, group.Name, group.Description);
            }
            [HttpGet("GetListOfGroups")]
            public async Task<IActionResult> GetListOfGroups(){
                return await _groupsService.GetListOfGroupAsync();
            }


    }



}