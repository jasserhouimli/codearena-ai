using Microsoft.AspNetCore.Identity;
using Modules.Common.Features.Abstractions;
using Modules.Common.Features.Results;
using Modules.Identity.Domain.Entities;

namespace Modules.Identity.Features.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, UserProfileResponse>
{
    private readonly UserManager<User> _userManager;

    public GetCurrentUserQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserProfileResponse>> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(query.UserId.ToString());
        if (user is null)
        {
            return Result.Failure<UserProfileResponse>(
                Error.NotFound("Identity.UserNotFound", $"User with ID '{query.UserId}' was not found."));
        }

        var roles = await _userManager.GetRolesAsync(user);

        var profile = new UserProfileResponse(
            user.Id,
            user.Email ?? string.Empty,
            user.FullName,
            user.GitHubUsername,
            user.AvatarUrl,
            user.Bio,
            roles.ToList(),
            user.CreatedAtUtc);

        return Result.Success(profile);
    }
}
