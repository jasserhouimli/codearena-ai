using Microsoft.Extensions.DependencyInjection;

namespace Modules.Recruitment.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddRecruitmentModule(this IServiceCollection services)
    {
        return services;
    }
}
