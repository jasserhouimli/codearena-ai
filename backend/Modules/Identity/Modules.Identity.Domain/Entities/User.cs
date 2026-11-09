namespace Modules.Identity.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? GitHubUsername { get; set; }
    public string? GitHubId { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
