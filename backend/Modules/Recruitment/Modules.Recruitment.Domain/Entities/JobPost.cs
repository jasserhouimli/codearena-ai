using Modules.Recruitment.Domain.Enums;

namespace Modules.Recruitment.Domain.Entities;

public class JobPost
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RecruiterId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public List<string> TechStack { get; set; } = new();

    public decimal? SalaryMin { get; set; }
    public decimal? SalaryMax { get; set; }
    public string Currency { get; set; } = "USD";
    public string Location { get; set; } = "Remote";

    public JobStatus Status { get; set; } = JobStatus.Draft;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }

    public ICollection<Application> Applications { get; set; } = new List<Application>();
}
