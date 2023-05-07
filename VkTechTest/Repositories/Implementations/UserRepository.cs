using Microsoft.EntityFrameworkCore;
using VkTechTest.Database;
using VkTechTest.Database.Models;
using VkTechTest.Models.Exceptions;
using VkTechTest.Repositories.Interfaces;

namespace VkTechTest.Repositories.Implementations;

public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationContext _applicationContext;

    public UserRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<UserEntity> SaveUserAsync(UserEntity userEntity)
    {
        var userExists = await _applicationContext.Users.AnyAsync(u => u.Login == userEntity.Login);

        if (userExists)
        {
            throw new UserAlreadyExistsException(userEntity.Id);
        }

        _applicationContext.Users.Add(userEntity);
        await _applicationContext.SaveChangesAsync();

        return userEntity;
    }

    public async Task<UserEntity?> GetUserByLoginAsync(string userLogin)
    {
        return await _applicationContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Login == userLogin);
    }
}