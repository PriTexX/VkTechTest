using Microsoft.EntityFrameworkCore;

namespace VkTechTest.Tests.Services;

public class UserServiceTests : DatabaseTests
{
    [Fact]
    public async Task TestConnection()
    {
        await using var ctx = CreateDbContext();

        var user = await ctx.Users.FirstAsync(u => u.Login == "admin");
        
        Assert.Equal("admin", user.Login);
    }
}