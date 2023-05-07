namespace VkTechTest.Models.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(long userId) : base($"User with id: {userId} already exists")
    {}
}