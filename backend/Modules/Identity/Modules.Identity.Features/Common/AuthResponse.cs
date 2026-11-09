namespace Modules.Identity.Features.Common;

public record AuthResponse(
    Guid UserId,
    string Email,
    string FullName,
    string? GitHubUsername,
    string? AvatarUrl,
    IReadOnlyList<string> Roles,
    string Token);
