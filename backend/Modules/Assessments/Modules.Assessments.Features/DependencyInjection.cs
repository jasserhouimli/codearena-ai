using Microsoft.Extensions.DependencyInjection;

namespace Modules.Assessments.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddAssessmentsModule(this IServiceCollection services)
    {
        return services;
    }
}
