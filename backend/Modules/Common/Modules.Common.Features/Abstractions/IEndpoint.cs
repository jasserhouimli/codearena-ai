using Microsoft.AspNetCore.Routing;

namespace Modules.Common.Features.Abstractions;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
