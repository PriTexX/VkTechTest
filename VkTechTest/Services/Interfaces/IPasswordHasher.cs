namespace VkTechTest.Services.Interfaces;

internal interface IPasswordHasher
{
    public string GetHash(string password);

    public bool Verify(string hash, string password);
}