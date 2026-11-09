using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Common.Features.Abstractions;
using Modules.Common.Features.Results;
using Modules.Identity.Features.Common;

namespace Modules.Identity.Features.GitHubOAuth;

internal sealed class GitHubOAuthEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/github", async (
            GitHubOAuthCommand command,
            IValidator<GitHubOAuthCommand> validator,
            ICommandHandler<GitHubOAuthCommand, AuthResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(command, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(
                response => Results.Ok(response),
                failure => CustomResults.Problem(failure));
        })
        .WithName("GitHubOAuth")
        .WithTags("Authentication")
        .Produces<AuthResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .AllowAnonymous();
    }
}
