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
            private readonly ILogger<GroupService> _logger;

            public GroupService (AppDbContext dbContext, ILogger<GroupService> logger)
            {
                _dbContext = dbContext;
                _logger = logger;
            }

            public async Task<IActionResult> CreateGroupAsync(string inputName, string inputDescription)
            {
                 try 
                    {
                    var group = new Group
                    {
                        Name = inputName,
                        Description = inputDescription
                    };

                   
                        _dbContext.Groups.Add(group);
                        await _dbContext.SaveChangesAsync();
                        _logger.LogInformation($"Group created: {group.Name} (ID: {group.Id})");
                            return new OkResult();
                    }
                    catch (DbUpdateException ex)
                    {
                     _logger.LogWarning(ex, "DbUpdateException occurred while creating group {GroupName}.", inputName);
                        return new BadRequestObjectResult("Cannot create group. Ensure data constraints are met (e.g. unique name).");
                    }
                    catch (Exception ex)
                    {
                      _logger.LogError(ex, "An unexpected error occurred while creating a group.");
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    }

                    
            }

            public async Task<IActionResult> DeleteGroupAsync(int inputId)
            {

                try{
                    
                    var group = await _dbContext.Groups.FindAsync(inputId);

                    if(group == null)
                    {
                        _logger.LogWarning($"Attempted to delete non-existent group with ID {inputId}.");
                        return new NotFoundResult();
                    }
                    
                    _dbContext.Groups.Remove(group);
                    
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInformation($"Group deleted: {group.Name} (ID: {group.Id})");
                    return new OkResult();
                }
                catch (DbUpdateException ex)
                    {
                        _logger.LogError(ex, "Database error occurred while deleting group ID {GroupId}.", inputId);
                        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An unexpected error occurred.");
                        return new BadRequestResult();
                    }

                   

                

                
            }

            public async Task<IActionResult> UpdateGroupAsync(int inputId, string inputName, string inputDescription)
            {

                try{
                        var group = await _dbContext.Groups.FindAsync(inputId);

                        if(group == null)
                        {
                            _logger.LogWarning($"Attempted to update non-existent group with ID {inputId}.");
                            return new NotFoundResult();
                        }

                        group.Name = inputName;
                        group.Description = inputDescription;

                        _dbContext.Groups.Update(group);
                        await _dbContext.SaveChangesAsync();

                         _logger.LogInformation($"Group updated: {group.Name} (ID: {group.Id})");
                
                        return new OkResult();
                }
                catch (DbUpdateException ex)
                {
                   _logger.LogWarning(ex, "DbUpdateException occurred while updating group ID {GroupId}.", inputId);
                    return new BadRequestObjectResult("Cannot update group. Ensure data constraints are met.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred while updating group ID {GroupId}.", inputId);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                
            }
            
            public async Task<IActionResult> GetGroupAsync(int inputId)
            {
                try{
                         var group = await _dbContext.Groups.FindAsync(inputId);

                        if(group == null)
                        {
                            _logger.LogWarning($"Attempted to retrieve non-existent group with ID {inputId}.");
                            return new NotFoundResult();
                        }

                         _logger.LogInformation($"Group retrieved: {group.Name} (ID: {group.Id})");
                            return new OkObjectResult(group);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred while retrieving group ID {GroupId}.", inputId);
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

               
            }

            public async Task<IActionResult> GetListOfGroupAsync()
            {
                try{

                    var groups = await _dbContext.Groups.ToListAsync();
                    _logger.LogInformation($"Groups retrieved: {groups.Count} groups found.");
                    return new OkObjectResult(groups);

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred while retrieving groups list.");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                 
            }
    }



}