using Microsoft.EntityFrameworkCore;
using Modules.Identity.Infrastructure.Database;
using Modules.Identity.PublicApi;
using Modules.Identity.PublicApi.DTOs;

namespace Modules.Identity.Infrastructure.Services;

public class IdentityApi : IIdentityApi
{
    private readonly IdentityDbContext _context;

    public IdentityApi(IdentityDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

        return new UserDto(
            user.Id,
            user.Email,
            user.FullName,
            user.GitHubUsername,
            user.AvatarUrl,
            roles);
    }

    public async Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
    }
}
