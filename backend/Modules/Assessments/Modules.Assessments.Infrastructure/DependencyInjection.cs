using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Assessments.Infrastructure.Database;
using Modules.Assessments.Infrastructure.Services;
using Modules.Assessments.PublicApi;

namespace Modules.Assessments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAssessmentsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AssessmentsDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Assessments"))
                   .UseSnakeCaseNamingConvention());

        services.AddScoped<IAssessmentsApi, AssessmentsApi>();

        return services;
    }
}
