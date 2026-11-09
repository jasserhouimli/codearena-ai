using FluentValidation;

namespace Modules.Identity.Features.GitHubOAuth;

public sealed class GitHubOAuthValidator : AbstractValidator<GitHubOAuthCommand>
{
    public GitHubOAuthValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("GitHub OAuth authorization code is required.");
    }
}
