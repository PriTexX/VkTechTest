using VkTechTest.Models.Enums;
using VkTechTest.Services.Interfaces;

namespace VkTechTest.Services.Implementations;

public sealed class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public Task<int> Register(string login, string password, UserGroupType userGroupType, UserStateType userStateType)
    {
        throw new NotImplementedException();
    }

    public Task<int> Login(string login, string password)
    {
        throw new NotImplementedException();
    }
}