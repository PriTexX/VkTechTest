using VkTechTest.Database.Models;

namespace VkTechTest.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<UserEntity> SaveUserAsync(UserEntity userEntity);

    public Task<UserEntity?> GetUserByLoginAsync(string userLogin);

    public Task<UserEntity?> GetUserWithStateAndGroupByLoginAsync(string login);

    public IAsyncEnumerable<UserEntity> GetAllUsersWithStateAndGroupAsync();

    public Task DeleteUserByLoginAsync(string login);
}