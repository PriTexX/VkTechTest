using Microsoft.AspNetCore.Mvc;

namespace VkTechTest.Contracts;

public sealed class DeleteUserRequest
{
    [FromRoute]
    public required string Login { get; init; }
}