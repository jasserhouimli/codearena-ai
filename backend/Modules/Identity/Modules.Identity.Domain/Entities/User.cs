using Microsoft.AspNetCore.Identity;

namespace Modules.Identity.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; } = string.Empty;
    public string? GitHubUsername { get; set; }
    public string? GitHubId { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public static User Create(
        string email,
        string fullName,
        string? gitHubUsername = null,
        string? gitHubId = null,
        string? avatarUrl = null)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = email,
            FullName = fullName,
            GitHubUsername = gitHubUsername,
            GitHubId = gitHubId,
            AvatarUrl = avatarUrl,
            EmailConfirmed = true,
            CreatedAtUtc = DateTime.UtcNow,
            IsActive = true
        };
    }
}
