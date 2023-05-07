using Microsoft.AspNetCore.Mvc;

namespace VkTechTest.Contracts.GetUserEndpoint;

public sealed class GetUserRequest
{
    [FromRoute]
    public required string Login { get; init; }
}