using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Modules.Common.Features.Abstractions;
using Modules.Common.Features.Results;
using Modules.Identity.Domain.Entities;
using Modules.Identity.Features.Common;
using Modules.Identity.Infrastructure.Services;

namespace Modules.Identity.Features.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        UserManager<User> userManager,
        IJwtTokenService jwtTokenService,
        ILogger<LoginCommandHandler> logger)
    {
        _userManager = userManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing login request for: {Email}", command.Email);

        var user = await _userManager.FindByEmailAsync(command.Email);
        if (user is null)
        {
            return Result.Failure<AuthResponse>(
                Error.Unauthorized("Identity.InvalidCredentials", "Invalid email or password."));
        }

        if (!user.IsActive)
        {
            return Result.Failure<AuthResponse>(
                Error.Forbidden("Identity.AccountInactive", "Your account has been deactivated."));
        }

        bool isValidPassword = await _userManager.CheckPasswordAsync(user, command.Password);
        if (!isValidPassword)
        {
            _logger.LogWarning("Invalid password attempt for user: {Email}", command.Email);
            return Result.Failure<AuthResponse>(
                Error.Unauthorized("Identity.InvalidCredentials", "Invalid email or password."));
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

        _logger.LogInformation("User logged in successfully: {UserId}", user.Id);

        return Result.Success(response);
    }
}
