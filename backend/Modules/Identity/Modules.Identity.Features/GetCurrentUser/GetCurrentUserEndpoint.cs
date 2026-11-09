using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modules.Common.Features.Abstractions;
using Modules.Common.Features.Results;

namespace Modules.Identity.Features.GetCurrentUser;

internal sealed class GetCurrentUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/auth/me", async (
            ClaimsPrincipal claimsPrincipal,
            IQueryHandler<GetCurrentUserQuery, UserProfileResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? claimsPrincipal.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Results.Unauthorized();
            }

            var query = new GetCurrentUserQuery(userId);
            var result = await handler.Handle(query, cancellationToken);

            return result.Match(
                response => Results.Ok(response),
                failure => CustomResults.Problem(failure));
        })
        .WithName("GetCurrentUser")
        .WithTags("Authentication")
        .Produces<UserProfileResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization();
    }
}
