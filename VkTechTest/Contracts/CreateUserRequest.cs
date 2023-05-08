namespace VkTechTest.Contracts;

/// <summary>
/// Запрос на создание нового пользователя
/// </summary>
public sealed class CreateUserRequest
{
    /// <summary>
    /// Логин
    /// </summary>
    public required string Login { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    public required string Password { get; init; }
}