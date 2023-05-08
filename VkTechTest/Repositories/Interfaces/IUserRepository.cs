﻿using VkTechTest.Database.Models;
using VkTechTest.Models.Enums;

namespace VkTechTest.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<UserEntity> SaveUserAsync(UserEntity userEntity);

    public Task<UserEntity?> GetUserByLoginAsync(string userLogin);

    public Task<UserEntity?> GetUserWithStateAndGroupByLoginAsync(string login);

    public IAsyncEnumerable<UserEntity> GetAllUsersWithStateAndGroupAsync(int take, int offset);

    public Task ChangeUserStateAsync(string login, long userStateId);

    public Task<long> GetStateIdAsync(UserStateType userStateType);
}