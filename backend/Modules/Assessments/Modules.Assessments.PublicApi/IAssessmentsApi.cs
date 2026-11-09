using Modules.Assessments.PublicApi.DTOs;

namespace Modules.Assessments.PublicApi;

public interface IAssessmentsApi
{
    Task<AssessmentSummaryDto?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
