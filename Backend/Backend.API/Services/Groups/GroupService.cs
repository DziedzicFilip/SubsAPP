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

            public async Task<IActionResult> DeleteGroupAsync(int inputId)
            {
                var group = await _dbContext.Groups.FindAsync(inputId);
                if (group == null)
                {
                    return new NotFoundResult();
                }
                

                _dbContext.Groups.Remove(group);
                await _dbContext.SaveChangesAsync();

                return new OkResult();
            }

            public async Task<IActionResult> UpdateGroupAsync(int inputId, string inputName, string inputDescription)
            {
                var group = await _dbContext.Groups.FindAsync(inputId);
                if(group == null)
                {
                    return new NotFoundResult();
                }

                group.Name = inputName;
                group.Description = inputDescription;

                _dbContext.Groups.Update(group);
                await _dbContext.SaveChangesAsync();

                return new OkResult();
            }
            
            public async Task<IActionResult> GetGroupAsync(int inputId)
            {
                var group = await _dbContext.Groups.FindAsync(inputId);
                if(group == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(group);
            }

            public async Task<IActionResult> GetListOfGroupAsync()
            {
                var groups = await _dbContext.Groups.ToListAsync();
                return new OkObjectResult(groups);
            }
    }



}