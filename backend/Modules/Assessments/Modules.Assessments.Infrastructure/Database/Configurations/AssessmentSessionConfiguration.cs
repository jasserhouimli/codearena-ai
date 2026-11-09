using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Assessments.Domain.Entities;

namespace Modules.Assessments.Infrastructure.Database.Configurations;

public class AssessmentSessionConfiguration : IEntityTypeConfiguration<AssessmentSession>
{
    public void Configure(EntityTypeBuilder<AssessmentSession> builder)
    {
        builder.ToTable("assessment_sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.ApplicationId)
            .IsRequired();

        builder.Property(s => s.CandidateId)
            .IsRequired();

        builder.Property(s => s.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.Feedback)
            .HasMaxLength(2000);

        builder.Property(s => s.MetadataJson)
            .HasColumnType("jsonb");

        builder.Property(s => s.KeystrokeLogsJson)
            .HasColumnType("jsonb");

        builder.HasIndex(s => s.ApplicationId);
        builder.HasIndex(s => s.CandidateId);
        builder.HasIndex(s => s.Status);
    }
}
