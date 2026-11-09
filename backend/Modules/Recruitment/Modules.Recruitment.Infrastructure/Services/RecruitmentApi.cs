using Microsoft.EntityFrameworkCore;
using Modules.Recruitment.Domain.Enums;
using Modules.Recruitment.Infrastructure.Database;
using Modules.Recruitment.PublicApi;
using Modules.Recruitment.PublicApi.DTOs;

namespace Modules.Recruitment.Infrastructure.Services;

public class RecruitmentApi : IRecruitmentApi
{
    private readonly RecruitmentDbContext _context;

    public RecruitmentApi(RecruitmentDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationDto?> GetApplicationByIdAsync(Guid applicationId, CancellationToken cancellationToken = default)
    {
        var app = await _context.Applications
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == applicationId, cancellationToken);

        if (app is null)
        {
            return null;
        }

        return new ApplicationDto(
            app.Id,
            app.JobPostId,
            app.CandidateId,
            app.CurrentStage.ToString(),
            app.OverallScore);
    }

    public async Task<bool> UpdateStageAsync(Guid applicationId, string nextStage, double? score = null, CancellationToken cancellationToken = default)
    {
        var app = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == applicationId, cancellationToken);

        if (app is null)
        {
            return false;
        }

        if (Enum.TryParse<ApplicationStage>(nextStage, true, out var stage))
        {
            app.CurrentStage = stage;
            if (score.HasValue)
            {
                app.OverallScore = score.Value;
            }
            app.UpdatedAtUtc = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }
}
