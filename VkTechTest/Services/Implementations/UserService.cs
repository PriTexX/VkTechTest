using VkTechTest.Database.Models;
using VkTechTest.Models.Enums;
using VkTechTest.Repositories.Interfaces;
using VkTechTest.Services.Interfaces;

namespace VkTechTest.Services.Implementations;

public sealed class UserService : IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;

    public UserService(IPasswordHasher passwordHasher, IUserRepository userRepository)
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
    }

    public async Task<UserEntity> Register(string login, string password, UserGroupType userGroupType, UserStateType userStateType)
    {
        var hashedPassword = _passwordHasher.Hash(password);
        
        var user = new UserEntity
        {
            Login = login,
            Password = hashedPassword,
            UserGroupId = (int)UserGroupType.User,
            UserStateId = (int)UserStateType.Active,
        };

        return await _userRepository.SaveUserAsync(user); // Не ловлю тут возможное исключение, т.к. оно пойдет дальше по стеку вызова и будет обработано вызывающим кодом(в моем случае контроллером). 
    }

    public async Task<UserEntity?> Login(string login, string password)
    {
        var user = await _userRepository.GetUserByLoginAsync(login);

        if (user is null)
        {
            return null;
        }

        return _passwordHasher.Verify(user.Password, password) ? user : null;
    }
}