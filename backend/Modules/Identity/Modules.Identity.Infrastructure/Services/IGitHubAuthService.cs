namespace Modules.Identity.Infrastructure.Services;

public record GitHubUserProfile(
    string Id,
    string Login,
    string Name,
    string Email,
    string? AvatarUrl,
    string? Bio,
    string AccessToken);

public interface IGitHubAuthService
{
    Task<GitHubUserProfile?> ExchangeCodeForProfileAsync(string code, CancellationToken cancellationToken = default);
}
