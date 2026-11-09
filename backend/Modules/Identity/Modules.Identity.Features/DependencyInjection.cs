using Microsoft.Extensions.DependencyInjection;

namespace Modules.Identity.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityModule(this IServiceCollection services)
    {
        return services;
    }
}
