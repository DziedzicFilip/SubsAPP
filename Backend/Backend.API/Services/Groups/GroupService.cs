using Backend.API.Models.Entities;
using Backend.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Backend.API.Data;
using Microsoft.AspNetCore.Mvc;


namespace Backend.API.Services.Groups
{
    public class GroupService : IGroupsService
    {

            private readonly AppDbContext  _dbContext;

            public GroupService (AppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IActionResult> CreateGroupAsync(string inputName, string inputDescription)
            {
                    var group = new Group
                    {
                        Name = inputName,
                        Description = inputDescription
                    };

                    try 
                    {
                        _dbContext.Groups.Add(group);
                        await _dbContext.SaveChangesAsync();
                    }
                    catch (Exception)
                    {
                        return new BadRequestResult();
                    }

                    return new OkResult();
            }

    }

}