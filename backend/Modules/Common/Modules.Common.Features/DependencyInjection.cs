using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Modules.Common.Features.Abstractions;

namespace Modules.Common.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddCommonModule(
        this IServiceCollection services,
        Assembly[] assemblies)
    {
        services.AddValidatorsFromAssemblies(assemblies);

        // Register all ICommandHandler implementations
        foreach (var assembly in assemblies)
        {
            var handlerTypes = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface);

            foreach (var type in handlerTypes)
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsGenericType)
                    {
                        var genericDef = iface.GetGenericTypeDefinition();
                        if (genericDef == typeof(ICommandHandler<>) ||
                            genericDef == typeof(ICommandHandler<,>) ||
                            genericDef == typeof(IQueryHandler<,>))
                        {
                            services.AddScoped(iface, type);
                        }
                    }
                }
            }
        }

        return services;
    }
}
