namespace Modules.Assessments.PublicApi.DTOs;

public record AssessmentSummaryDto(
    Guid Id,
    Guid ApplicationId,
    Guid CandidateId,
    string Type,
    string Status,
    double? Score);
