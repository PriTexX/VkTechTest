namespace VkTechTest.Services.Interfaces;

public interface IPasswordHasher
{
    public string GetHash(string password);

    public bool Verify(string hash, string password);
}