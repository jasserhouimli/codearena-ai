namespace Modules.Recruitment.PublicApi.DTOs;

public record ApplicationDto(
    Guid Id,
    Guid JobPostId,
    Guid CandidateId,
    string CurrentStage,
    double? OverallScore);
