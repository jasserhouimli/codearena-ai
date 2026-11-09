using Microsoft.EntityFrameworkCore;
using Modules.Assessments.Infrastructure.Database;
using Modules.Assessments.PublicApi;
using Modules.Assessments.PublicApi.DTOs;

namespace Modules.Assessments.Infrastructure.Services;

public class AssessmentsApi : IAssessmentsApi
{
    private readonly AssessmentsDbContext _context;

    public AssessmentsApi(AssessmentsDbContext context)
    {
        _context = context;
    }

    public async Task<AssessmentSummaryDto?> GetSessionByIdAsync(Guid sessionId, CancellationToken cancellationToken = default)
    {
        var session = await _context.AssessmentSessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

        if (session is null)
        {
            return null;
        }

        return new AssessmentSummaryDto(
            session.Id,
            session.ApplicationId,
            session.CandidateId,
            session.Type.ToString(),
            session.Status.ToString(),
            session.Score);
    }
}
