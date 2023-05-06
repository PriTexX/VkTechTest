using VkTechTest.Models.Enums;

namespace VkTechTest.Services.Interfaces;

public interface IUserService
{
    public Task<int> Register(string login, string password, UserGroupType userGroupType, UserStateType userStateType);

    public Task<int> Login(string login, string password);
}