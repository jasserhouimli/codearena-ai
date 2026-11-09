using Modules.Assessments.Domain.Enums;

namespace Modules.Assessments.Domain.Entities;

public class AssessmentSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ApplicationId { get; set; }
    public Guid CandidateId { get; set; }

    public AssessmentType Type { get; set; }
    public AssessmentStatus Status { get; set; } = AssessmentStatus.Pending;

    public DateTime StartedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAtUtc { get; set; }

    public double? Score { get; set; }
    public string? Feedback { get; set; }

    // JSON payload storing AI defense questions/answers or coding task configuration
    public string? MetadataJson { get; set; }

    // Time-series keystroke diffs and event logs for replay inspector
    public string? KeystrokeLogsJson { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
}
