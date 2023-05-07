namespace VkTechTest.Models.Exceptions;

public class UserRegistrationDelayException : Exception
{
    public UserRegistrationDelayException() : base($"You have to wait 5 second before registering user with same login")
    {
    }
}