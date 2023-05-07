using VkTechTest.Contracts.Common;
using VkTechTest.Database.Models;

namespace VkTechTest.Mappers;

public static class UserMapper
{
    public static UserResponse MapFromDBUser(UserEntity dbUser)
    {
        return new UserResponse
        {
            Id = dbUser.Id,
            CreatedDate = dbUser.CreatedDate,
            Login = dbUser.Login,
            UserGroup = dbUser.UserGroup.Code,
            UserState = dbUser.UserState.Code
        };
    }

    public static async IAsyncEnumerable<UserResponse> MapFromDBUsers(IAsyncEnumerable<UserEntity> dbUsers)
    {
        await foreach (var user in dbUsers)
        {
            yield return MapFromDBUser(user);
        }
    }
}