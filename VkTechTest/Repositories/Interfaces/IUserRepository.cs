using VkTechTest.Database.Models;

namespace VkTechTest.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<UserEntity> SaveUserAsync(UserEntity userEntity);
}