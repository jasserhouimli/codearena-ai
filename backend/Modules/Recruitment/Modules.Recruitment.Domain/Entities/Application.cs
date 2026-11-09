using Modules.Recruitment.Domain.Enums;

namespace Modules.Recruitment.Domain.Entities;

public class Application
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid JobPostId { get; set; }
    public JobPost JobPost { get; set; } = null!;

    public Guid CandidateId { get; set; }

    public ApplicationStage CurrentStage { get; set; } = ApplicationStage.Applied;
    public double? OverallScore { get; set; }
    public string? ResumeUrl { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
}
