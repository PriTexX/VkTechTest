using Microsoft.AspNetCore.Mvc;

namespace VkTechTest.Contracts;

public sealed class GetUserRequest
{
    [FromRoute]
    public required string Login { get; init; }
}