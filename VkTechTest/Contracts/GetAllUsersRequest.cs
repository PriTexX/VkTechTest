namespace VkTechTest.Contracts;

public sealed class GetAllUsersRequest
{
    public required int PageSize { get; init; }
    public required int OffSet { get; init; }
}