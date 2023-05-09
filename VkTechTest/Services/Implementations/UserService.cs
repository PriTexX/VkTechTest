using VkTechTest.DAO.Interfaces;
using VkTechTest.Database.Models;
using VkTechTest.Models.Enums;
using VkTechTest.Models.Exceptions;
using VkTechTest.Services.Interfaces;

namespace VkTechTest.Services.Implementations;

public sealed class UserService : IUserService
{
    private static long BlockedStateId = -1;

    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserDAO _userDao;

    public UserService(IPasswordHasher passwordHasher, IUserDAO userDao)
    {
        _passwordHasher = passwordHasher;
        _userDao = userDao;
    }

    public async Task<UserEntity> RegisterAsync(string login, string password)
    {
        var dbUser = await _userDao.GetUserByLoginAsync(login);
        
        if (dbUser is not null && DateTime.UtcNow - dbUser.CreatedDate.ToUniversalTime() < TimeSpan.FromSeconds(5))
        {
            throw new UserRegistrationDelayException();
        }
        
        var hashedPassword = _passwordHasher.Hash(password);
        
        var newUser = new UserEntity
        {
            Login = login,
            Password = hashedPassword,
            UserGroupId = (int)UserGroupType.User,
            UserStateId = (int)UserStateType.Active,
        };
        
        return await _userDao.SaveUserAsync(newUser); // Не ловлю тут возможное исключение, т.к. оно пойдет дальше по стеку вызова и будет обработано вызывающим кодом(в моем случае контроллером). 
    }

    public async Task<UserEntity?> AuthenticateAsync(string login, string password)
    {
        var user = await _userDao.GetUserWithStateAndGroupByLoginAsync(login);

        if (user is null)
        {
            return null;
        }

        return _passwordHasher.Verify(user.Password, password) ? user : null;
    }

    public async Task RemoveUserAsync(string login)
    {
        if (BlockedStateId == -1)
        {
            BlockedStateId = await _userDao.GetStateIdAsync(UserStateType.Blocked);
        }

        await _userDao.ChangeUserStateAsync(login, BlockedStateId);
    }
}