using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Modules.Common.Features.Abstractions;
using Modules.Common.Features.Results;
using Modules.Identity.Domain.Entities;
using Modules.Identity.Features.Common;
using Modules.Identity.Infrastructure.Services;

namespace Modules.Identity.Features.Register;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResponse>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IJwtTokenService jwtTokenService,
        ILogger<RegisterCommandHandler> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Attempting registration for email: {Email}", command.Email);

        var existingUser = await _userManager.FindByEmailAsync(command.Email);
        if (existingUser is not null)
        {
            return Result.Failure<AuthResponse>(
                Error.Conflict("Identity.UserAlreadyExists", $"A user with email '{command.Email}' already exists."));
        }

        // Ensure target role exists
        if (!await _roleManager.RoleExistsAsync(command.Role))
        {
            await _roleManager.CreateAsync(new Role(command.Role));
        }

        var user = User.Create(command.Email, command.FullName);

        var identityResult = await _userManager.CreateAsync(user, command.Password);
        if (!identityResult.Succeeded)
        {
            var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
            _logger.LogWarning("User registration failed for {Email}: {Errors}", command.Email, errors);
            return Result.Failure<AuthResponse>(Error.Validation("Identity.RegistrationFailed", errors));
        }

        await _userManager.AddToRoleAsync(user, command.Role);

        string token = await _jwtTokenService.GenerateTokenAsync(user, cancellationToken);

        var response = new AuthResponse(
            user.Id,
            user.Email!,
            user.FullName,
            user.GitHubUsername,
            user.AvatarUrl,
            new[] { command.Role },
            token);

        _logger.LogInformation("User registered successfully: {UserId} with role {Role}", user.Id, command.Role);

        return Result.Success(response);
    }
}
