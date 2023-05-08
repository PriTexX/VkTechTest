namespace VkTechTest.Models.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string login) : base($"User with login: '{login}' was not found")
    {}
}