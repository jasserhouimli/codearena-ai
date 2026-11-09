using Microsoft.EntityFrameworkCore;
using Modules.Assessments.Domain.Entities;

namespace Modules.Assessments.Infrastructure.Database;

public class AssessmentsDbContext : DbContext
{
    public AssessmentsDbContext(DbContextOptions<AssessmentsDbContext> options)
        : base(options)
    {
    }

    public DbSet<AssessmentSession> AssessmentSessions => Set<AssessmentSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentsDbContext).Assembly);
    }
}
