using Modules.Identity.PublicApi.DTOs;

namespace Modules.Identity.PublicApi;

public interface IIdentityApi
{
    Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken = default);
}
