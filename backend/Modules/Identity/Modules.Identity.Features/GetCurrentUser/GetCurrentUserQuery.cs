using Modules.Common.Features.Abstractions;

namespace Modules.Identity.Features.GetCurrentUser;

public record UserProfileResponse(
    Guid Id,
    string Email,
    string FullName,
    string? GitHubUsername,
    string? AvatarUrl,
    string? Bio,
    IReadOnlyList<string> Roles,
    DateTime CreatedAtUtc);

public sealed record GetCurrentUserQuery(Guid UserId) : IQuery<UserProfileResponse>;
