using VkTechTest.Database.Models;

namespace VkTechTest.Services.Interfaces;

public interface IUserService
{
    public Task<UserEntity> RegisterAsync(string login, string password);

    public Task<UserEntity?> AuthenticateAsync(string login, string password);

    public Task RemoveUserAsync(string login);
}