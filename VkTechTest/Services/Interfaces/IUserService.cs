using VkTechTest.Database.Models;

namespace VkTechTest.Services.Interfaces;

public interface IUserService
{
    public Task<UserEntity> Register(string login, string password);

    public Task<UserEntity?> Login(string login, string password);
}