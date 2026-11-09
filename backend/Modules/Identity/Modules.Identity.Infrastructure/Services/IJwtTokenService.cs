using Modules.Identity.Domain.Entities;

namespace Modules.Identity.Infrastructure.Services;

public interface IJwtTokenService
{
    Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken = default);
}
