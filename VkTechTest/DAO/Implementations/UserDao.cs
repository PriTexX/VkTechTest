﻿using Microsoft.EntityFrameworkCore;
using VkTechTest.DAO.Interfaces;
using VkTechTest.Database;
using VkTechTest.Database.Models;
using VkTechTest.Models.Enums;
using VkTechTest.Models.Exceptions;

namespace VkTechTest.DAO.Implementations;

public sealed class UserDao : IUserDAO
{
    private readonly ApplicationContext _applicationContext;

    public UserDao(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public async Task<UserEntity> SaveUserAsync(UserEntity userEntity)
    {
        var userExists = await _applicationContext.Users.AnyAsync(u => u.Login == userEntity.Login);

        if (userExists)
        {
            throw new UserAlreadyExistsException(userEntity.Login);
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

    public async IAsyncEnumerable<UserEntity> GetAllUsersWithStateAndGroupAsync(int take, int offset)
    {
        var users = _applicationContext.Users
            .AsNoTrackingWithIdentityResolution()
            .Include(u => u.UserState)
            .Include(u => u.UserGroup)
            .OrderBy(u => u.Id)
            .Skip(offset)
            .Take(take)
            .AsAsyncEnumerable();
        
        await foreach (var user in users)
        {
            yield return user;
        }
    }

    public async Task ChangeUserStateAsync(string login, long userStateId)
    {
        var rowsAffected = await _applicationContext.Users
            .Where(u => u.Login == login)
            .ExecuteUpdateAsync(p => p
                .SetProperty(u => u.UserStateId, userStateId));

        if (rowsAffected == 0)
        {
            throw new UserNotFoundException(login);
        }
    }

    public async Task<long> GetStateIdAsync(UserStateType userStateType)
    {
        return await _applicationContext.UserStates
            .AsNoTracking()
            .Where(s => s.Code == userStateType)
            .Select(u => u.Id)
            .FirstAsync(); // Если в базе не окажется стейта, то в таком случае это исключительная ситуация. Приложение залогирует ошибку, вернет пользователю 500 код и продолжит дальше обрабатывать запросы 
    }
}