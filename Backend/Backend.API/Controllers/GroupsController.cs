using Backend.API.Services.Groups;
using Microsoft.AspNetCore.Mvc;
using Backend.API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

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

            [Authorize]
            [HttpPost("Group")]
            public async Task<IActionResult> CreateGroup([FromBody] CreateUpdateGroupDTO group)
            {

                return await _groupsService.CreateGroupAsync(group.Name, group.Description);

            }
             [Authorize]
            [HttpDelete("Group/{id}")]
            public async Task<IActionResult> DeleteGroup(int id)
            {

                return await _groupsService.DeleteGroupAsync(id);


            }
             [Authorize]
            [HttpGet("Group/{id}")]
            public async Task<IActionResult> GetGroup(int id)
            {

                return await _groupsService.GetGroupAsync(id);

            }
             [Authorize]
            [HttpPatch("Group/{id}")]
            public async Task<IActionResult> UpdateGroup(int id, [FromBody] CreateUpdateGroupDTO group)
            {

                return await _groupsService.UpdateGroupAsync(id, group.Name, group.Description);

            }
            
            [Authorize]
            [HttpGet("Groups")]
            public async Task<IActionResult> GetListOfGroups()
            {

                return await _groupsService.GetListOfGroupAsync();


            }


    }



}