using Modules.Recruitment.PublicApi.DTOs;

namespace Modules.Recruitment.PublicApi;

public interface IRecruitmentApi
{
    Task<ApplicationDto?> GetApplicationByIdAsync(Guid applicationId, CancellationToken cancellationToken = default);
    Task<bool> UpdateStageAsync(Guid applicationId, string nextStage, double? score = null, CancellationToken cancellationToken = default);
}
