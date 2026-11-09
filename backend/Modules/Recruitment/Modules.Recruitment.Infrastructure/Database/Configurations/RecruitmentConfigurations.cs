using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Recruitment.Domain.Entities;

namespace Modules.Recruitment.Infrastructure.Database.Configurations;

public class JobPostConfiguration : IEntityTypeConfiguration<JobPost>
{
    public void Configure(EntityTypeBuilder<JobPost> builder)
    {
        builder.ToTable("job_posts");

        builder.HasKey(jp => jp.Id);

        builder.Property(jp => jp.RecruiterId)
            .IsRequired();

        builder.Property(jp => jp.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(jp => jp.CompanyName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(jp => jp.Description)
            .IsRequired();

        builder.Property(jp => jp.Requirements)
            .IsRequired();

        builder.Property(jp => jp.SalaryMin)
            .HasPrecision(18, 2);

        builder.Property(jp => jp.SalaryMax)
            .HasPrecision(18, 2);

        builder.Property(jp => jp.Currency)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("USD");

        builder.Property(jp => jp.Location)
            .IsRequired()
            .HasMaxLength(100)
            .HasDefaultValue("Remote");

        builder.Property(jp => jp.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(jp => jp.RecruiterId);
        builder.HasIndex(jp => jp.Status);
    }
}

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("applications");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.CandidateId)
            .IsRequired();

        builder.Property(a => a.CurrentStage)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(a => a.ResumeUrl)
            .HasMaxLength(500);

        builder.Property(a => a.Notes)
            .HasMaxLength(2000);

        builder.HasOne(a => a.JobPost)
            .WithMany(jp => jp.Applications)
            .HasForeignKey(a => a.JobPostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(a => new { a.JobPostId, a.CandidateId })
            .IsUnique();

        builder.HasIndex(a => a.CurrentStage);
    }
}
