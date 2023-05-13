namespace VkTechTest.Models.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string userLogin) : base($"User with login: '{userLogin}' already exists")
    {}
}