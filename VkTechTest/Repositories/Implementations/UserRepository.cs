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

    public async Task<UserEntity?> GetUserWithStateAndGroupByLoginAsync(string login)
    {
        return await _applicationContext.Users
            .AsNoTracking()
            .Include(u => u.UserState)
            .Include(u => u.UserGroup)
            .FirstOrDefaultAsync(u => u.Login == login);
    }

    public async IAsyncEnumerable<UserEntity> GetAllUsersWithStateAndGroupAsync()
    {
        var users = _applicationContext.Users
            .AsNoTrackingWithIdentityResolution()
            .Include(u => u.UserState)
            .Include(u => u.UserGroup)
            .AsAsyncEnumerable();
        
        await foreach (var user in users)
        {
            yield return user;
        }
    }

    public async Task DeleteUserByLoginAsync(string login)
    {
        var rowsDeleted = await _applicationContext.Users
            .Where(u => u.Login == login)
            .ExecuteDeleteAsync();

        if (rowsDeleted == 0)
        {
            throw new UserNotFoundException(login);
        }
    }
}