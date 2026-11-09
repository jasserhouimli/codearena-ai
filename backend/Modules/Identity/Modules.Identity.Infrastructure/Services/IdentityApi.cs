using Microsoft.AspNetCore.Identity;
using Modules.Identity.Domain.Entities;
using Modules.Identity.PublicApi;
using Modules.Identity.PublicApi.DTOs;

namespace Modules.Identity.Infrastructure.Services;

public class IdentityApi : IIdentityApi
{
    private readonly UserManager<User> _userManager;

    public IdentityApi(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return null;
        }

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto(
            user.Id,
            user.Email ?? string.Empty,
            user.FullName,
            user.GitHubUsername,
            user.AvatarUrl,
            roles.ToList());
    }

    public async Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user is not null;
    }
}
