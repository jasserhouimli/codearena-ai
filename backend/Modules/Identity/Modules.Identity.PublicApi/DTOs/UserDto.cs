namespace Modules.Identity.PublicApi.DTOs;

public record UserDto(
    Guid Id,
    string Email,
    string FullName,
    string? GitHubUsername,
    string? AvatarUrl,
    IReadOnlyList<string> Roles);
