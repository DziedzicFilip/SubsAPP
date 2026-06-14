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
            public async Task<IActionResult> CreateGroup([FromBody] GroupDTO group)
            {
                return await _groupsService.CreateGroupAsync(group.Name, group.Description);
            }
    }

}