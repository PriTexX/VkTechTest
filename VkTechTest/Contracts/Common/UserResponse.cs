using System.Text.Json.Serialization;
using VkTechTest.Models.Enums;

namespace VkTechTest.Contracts.Common;

public sealed class UserResponse
{
    [JsonPropertyName("id")]
    public required long Id { get; init; }
    
    [JsonPropertyName("login")]
    public required string Login { get; init; }
    
    [JsonPropertyName("created_date")]
    public required DateTime CreatedDate { get; init; }
    
    [JsonPropertyName("user_state")]
    public required UserStateType UserState { get; init; }

    [JsonPropertyName("user_group")]
    public required UserGroupType UserGroup { get; init; }
}