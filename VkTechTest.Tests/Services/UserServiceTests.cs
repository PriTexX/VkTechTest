using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VkTechTest.DAO.Implementations;
using VkTechTest.Models.Enums;
using VkTechTest.Models.Exceptions;
using VkTechTest.Services.Implementations;
using Xunit.Abstractions;
using Xunit.Sdk;
using static VkTechTest.Tests.PostgresContainerManager;

namespace VkTechTest.Tests.Services;


// SUT - System Under Test
public class UserServiceTests : DatabaseTestsHelper
{
    [Fact]
    public async Task Registering_New_User_Is_Succesfull()
    {
        // Arrange
        
        await using var ctx = CreateDbContext();

        await ClearDatabase(ctx);
        
        var userDao = new UserDao(ctx);
        var sut = new UserService(new SHA256PasswordHasher(), userDao);

        // Act

        await sut.RegisterAsync("testUser", "withTestPassword");
        
        // Assert

        var createdUser = await ctx.Users
            .Include(u => u.UserGroup)
            .Include(u => u.UserState)
            .FirstAsync(u => u.Login == "testUser");

        createdUser.UserState.Code.Should().Be(UserStateType.Active);
        createdUser.UserGroup.Code.Should().Be(UserGroupType.User);
    }
    
    [Fact]
    public async Task Attempt_To_Registrate_Not_After_5sec_Delay_Should_Throw_Exception()
    {
        // Arrange

        await using var ctx = CreateDbContext();
        
        await ClearDatabase(ctx);
        
        var userDao = new UserDao(ctx);
        var sut = new UserService(new SHA256PasswordHasher(), userDao);
        await sut.RegisterAsync("testUser", "withTestPassword");
        
        // Act

        var act = async () => await sut.RegisterAsync("testUser", "withDifferentPassword");
        
        // Assert

        await act.Should().ThrowAsync<UserRegistrationDelayException>();
    }

    [Fact]
    public async Task Attempt_To_Registrate_With_Existed_Login_Should_Throw_Exception()
    {
        // Arrange

        await using var ctx = CreateDbContext();
        
        await ClearDatabase(ctx);
        
        var userDao = new UserDao(ctx);
        var sut = new UserService(new SHA256PasswordHasher(), userDao);
        await sut.RegisterAsync("testUser", "withTestPassword");
        Thread.Sleep(5001); 
        
        // Act

        var act = async () => await sut.RegisterAsync("testUser", "withDifferentPassword");
        
        // Assert

        await act.Should().ThrowAsync<UserAlreadyExistsException>();
    }

    [Fact]
    public async Task Remove_User_Should_Change_His_State_To_Blocked()
    {
        // Arrange

        await using var ctx = CreateDbContext();
        
        await ClearDatabase(ctx);
        
        var userDao = new UserDao(ctx);
        var sut = new UserService(new SHA256PasswordHasher(), userDao);
        await sut.RegisterAsync("testUser", "withTestPassword");

        // Act
        
        await sut.RemoveUserAsync("testUser");
        
        // Assert

        var user = await ctx.Users
            .AsNoTracking()
            .Include(u => u.UserState)
            .FirstAsync(u => u.Login == "testUser");
        
        user.UserState.Code.Should().Be(UserStateType.Blocked);
    }

    [Fact]
    public async Task Remove_UnExisting_User_Should_Throw_Exception()
    {
        // Arrange

        await using var ctx = CreateDbContext();
        
        await ClearDatabase(ctx);
        
        var userDao = new UserDao(ctx);
        var sut = new UserService(new SHA256PasswordHasher(), userDao);
        
        // Act
        
        var act = async () => await sut.RemoveUserAsync("testUser");
        
        // Assert

        await act.Should().ThrowAsync<UserNotFoundException>();
    }
}