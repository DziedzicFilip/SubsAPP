using Backend.API.Models.Entities;
using Backend.API.Services;
using Moq;
using Xunit;
using Backend.API.Models.DTOs;
using Microsoft.Extensions.Logging;
using Backend.API.Data;
using Backend.API.Services.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Tests.ServicesTests
{
    public class GroupServiceTest : IDisposable
    {
        private readonly IGroupsService _GroupService;
        private readonly AppDbContext _dbContext;
        private readonly Mock<ILogger<GroupService>> _mockLogger;

        public GroupServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new AppDbContext(options);
            _mockLogger = new Mock<ILogger<GroupService>>();
            _GroupService = new GroupService(_dbContext, _mockLogger.Object);
        }


        [Fact]
        public async Task CreateGroupAsync_ShouldReturnOkResult_WhenGroupIsCreated()
        {
            var newGroup = new CreateGroupDTO
            {
                Name = "Test Group",
                Description = "This is a test group."
            };

            var result = await _GroupService.CreateGroupAsync(newGroup.Name, newGroup.Description);

            var groupInDb = await _dbContext.Groups.FirstOrDefaultAsync(g => g.Name == newGroup.Name);

            Assert.True(result is OkResult);
            Assert.NotNull(groupInDb);
            Assert.Equal(newGroup.Name, groupInDb.Name);
            Assert.Equal(newGroup.Description, groupInDb.Description);

        }

        [Fact]
        public async Task DeleteGroupAsync_ShouldReturnOkResult_WhenGroupIsDeleted()
        {
            var group = new Group
            {
                Id = 1,
                Name = "Group to Delete",
                Description = "This group will be deleted."
            };
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            var result = await _GroupService.DeleteGroupAsync(group.Id);

            var groupInDb = await _dbContext.Groups.FindAsync(group.Id);

            Assert.True(result is OkResult);
            Assert.Null(groupInDb);


        }

        [Fact]
        public async Task DeleteGroupAsync_shouldReturnNotFoundResult_WhenGroupDoesNotExist()
        {
            var listOfGroups = await _dbContext.Groups.ToListAsync();


            var nonExistentGroupId = listOfGroups.Any() ? listOfGroups.Max(g => g.Id) + 1 : 1;

            var result = await _GroupService.DeleteGroupAsync(nonExistentGroupId);

            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public async Task CreateGroupAsync_ShouldReturnBadRequest_WhenGroupNameIsNotUnique()
        {

            var existingGroup = new Group
            {
                Name = "Existing Group",
                Description = "This group already exists."
            };
            await _dbContext.Groups.AddAsync(existingGroup);
            await _dbContext.SaveChangesAsync();

            var result = await _GroupService.CreateGroupAsync(existingGroup.Name, "Trying to create a duplicate group.");

            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public async Task UpdateGroupAsync_ShouldReturnBadRequest_WhenGroupNameIsNotUnique()
        {
            var existingGroup1 = new Group
            {
                Name = "Existing Group 1",
                Description = "This group already exists."
            };
            var existingGroup2 = new Group
            {
                Name = "Existing Group 2",
                Description = "This group also already exists."
            };

            _dbContext.Groups.AddRange(existingGroup1, existingGroup2);
            await _dbContext.SaveChangesAsync();

            var result = await _GroupService.UpdateGroupAsync(existingGroup1.Id, existingGroup2.Name, "Trying to update to a duplicate name.");

            Assert.True(result is BadRequestObjectResult);
        }

        [Fact]
        public async Task UpdateGroupAsync_ShouldReturnOkResult_WhenGroupIsUpdated()
        {
            var existingGroup = new Group
            {
                Name = "Group to Update",
                Description = "This group will be updated."
            };
            await _dbContext.Groups.AddAsync(existingGroup);
            await _dbContext.SaveChangesAsync();

            var result = await _GroupService.UpdateGroupAsync(existingGroup.Id, "Updated Group Name", "Updated group description.");

            Assert.True(result is OkResult);
        }

        [Fact]
        public async Task UpdateGroupAsync_ShouldReturnNotFoundResult_WhenIDDoesNotExist()
        {
            var listOfGroups = await _dbContext.Groups.ToListAsync();
            var nonExistentGroupId = listOfGroups.Any() ? listOfGroups.Max(g => g.Id) + 1 : 1;


            var result = await _GroupService.UpdateGroupAsync(nonExistentGroupId, "New Name", "New Description");

            Assert.True(result is NotFoundResult);
        }

        [Fact]
        public async Task GetGroupByIdAsync_ShouldReturnGroup_WhenGroupExists()
        {
            var group = new Group
            {
                Name = "Test Group",
                Description = "This is a test group."
            };
            await _dbContext.Groups.AddAsync(group);
            await _dbContext.SaveChangesAsync();

            var result = await _GroupService.GetGroupAsync(group.Id);

            Assert.NotNull(result);
           
        }

        [Fact]
        public async Task GetGroupByIdAsync_ShouldReturnNotFoundResult_WhenGroupDoesNotExist()
        {
            var listOfGroups = await _dbContext.Groups.ToListAsync();
            var nonExistentGroupId = listOfGroups.Any() ? listOfGroups.Max(g => g.Id) + 1 : 1;

            var result = await _GroupService.GetGroupAsync(nonExistentGroupId);

            Assert.True(result is NotFoundResult);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

    }
}