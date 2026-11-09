using Microsoft.EntityFrameworkCore;
using Modules.Recruitment.Domain.Entities;

namespace Modules.Recruitment.Infrastructure.Database;

public class RecruitmentDbContext : DbContext
{
    public RecruitmentDbContext(DbContextOptions<RecruitmentDbContext> options)
        : base(options)
    {
    }

    public DbSet<JobPost> JobPosts => Set<JobPost>();
    public DbSet<Application> Applications => Set<Application>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecruitmentDbContext).Assembly);
    }
}
