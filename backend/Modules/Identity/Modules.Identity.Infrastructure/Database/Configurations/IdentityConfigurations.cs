using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Identity.Domain.Entities;

namespace Modules.Identity.Infrastructure.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.GitHubUsername)
            .HasMaxLength(100);

        builder.HasIndex(u => u.GitHubUsername);

        builder.Property(u => u.GitHubId)
            .HasMaxLength(100);

        builder.Property(u => u.AvatarUrl)
            .HasMaxLength(500);

        builder.Property(u => u.Bio)
            .HasMaxLength(1000);

        builder.Property(u => u.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedAtUtc)
            .IsRequired();
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Description)
            .HasMaxLength(250);

        builder.HasData(
            new Role
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = Domain.Constants.Roles.Admin,
                NormalizedName = Domain.Constants.Roles.Admin.ToUpperInvariant(),
                Description = "System Administrator"
            },
            new Role
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = Domain.Constants.Roles.Recruiter,
                NormalizedName = Domain.Constants.Roles.Recruiter.ToUpperInvariant(),
                Description = "Recruiter / Technical Hiring Manager"
            },
            new Role
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = Domain.Constants.Roles.Candidate,
                NormalizedName = Domain.Constants.Roles.Candidate.ToUpperInvariant(),
                Description = "Candidate / Developer"
            }
        );
    }
}
