using VkTechTest.Database.Models;
using VkTechTest.Models.Enums;

namespace VkTechTest.Services.Interfaces;

public interface IUserService
{
    public Task<UserEntity> Register(string login, string password, UserGroupType userGroupType, UserStateType userStateType);

    public Task<UserEntity?> Login(string login, string password);
}