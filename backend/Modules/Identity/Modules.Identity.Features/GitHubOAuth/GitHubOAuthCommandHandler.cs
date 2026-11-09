using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Common.Features.Abstractions;
using Modules.Common.Features.Results;
using Modules.Identity.Domain.Constants;
using Modules.Identity.Domain.Entities;
using Modules.Identity.Features.Common;
using Modules.Identity.Infrastructure.Services;

namespace Modules.Identity.Features.GitHubOAuth;

internal sealed class GitHubOAuthCommandHandler : ICommandHandler<GitHubOAuthCommand, AuthResponse>
{
    private readonly IGitHubAuthService _gitHubAuthService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<GitHubOAuthCommandHandler> _logger;

    public GitHubOAuthCommandHandler(
        IGitHubAuthService gitHubAuthService,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IJwtTokenService jwtTokenService,
        ILogger<GitHubOAuthCommandHandler> logger)
    {
        _gitHubAuthService = gitHubAuthService;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(GitHubOAuthCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing GitHub OAuth authentication callback.");

        var gitHubProfile = await _gitHubAuthService.ExchangeCodeForProfileAsync(command.Code, cancellationToken);
        if (gitHubProfile is null)
        {
            _logger.LogError("Failed to retrieve profile from GitHub.");
            return Result.Failure<AuthResponse>(
                Error.Unauthorized("GitHub.AuthFailed", "Failed to authenticate with GitHub. Invalid or expired code."));
        }

        // Look up candidate by GitHub Id or Email
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.GitHubId == gitHubProfile.Id || u.Email == gitHubProfile.Email, cancellationToken);

        if (user is null)
        {
            // Provision new candidate user
            user = User.Create(
                email: gitHubProfile.Email,
                fullName: gitHubProfile.Name,
                gitHubUsername: gitHubProfile.Login,
                gitHubId: gitHubProfile.Id,
                avatarUrl: gitHubProfile.AvatarUrl);

            user.Bio = gitHubProfile.Bio;

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                _logger.LogError("Failed to provision new GitHub candidate: {Errors}", errors);
                return Result.Failure<AuthResponse>(Error.Failure("Identity.ProvisionFailed", errors));
            }

            // Ensure Candidate role exists
            if (!await _roleManager.RoleExistsAsync(Roles.Candidate))
            {
                await _roleManager.CreateAsync(new Role(Roles.Candidate));
            }

            await _userManager.AddToRoleAsync(user, Roles.Candidate);
            _logger.LogInformation("New developer candidate provisioned from GitHub: {UserId} ({GitHubUsername})", user.Id, user.GitHubUsername);
        }
        else
        {
            // Update existing user profile metadata
            user.GitHubId = gitHubProfile.Id;
            user.GitHubUsername = gitHubProfile.Login;
            user.AvatarUrl = gitHubProfile.AvatarUrl;
            user.Bio = gitHubProfile.Bio ?? user.Bio;
            user.UpdatedAtUtc = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            _logger.LogInformation("Existing candidate profile updated from GitHub: {UserId}", user.Id);
        }

        var roles = await _userManager.GetRolesAsync(user);
        string token = await _jwtTokenService.GenerateTokenAsync(user, cancellationToken);

        var response = new AuthResponse(
            user.Id,
            user.Email!,
            user.FullName,
            user.GitHubUsername,
            user.AvatarUrl,
            roles.ToList(),
            token);

        return Result.Success(response);
    }
}
