using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Recruitment.Infrastructure.Database;
using Modules.Recruitment.Infrastructure.Services;
using Modules.Recruitment.PublicApi;

namespace Modules.Recruitment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddRecruitmentInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<RecruitmentDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Recruitment"))
                   .UseSnakeCaseNamingConvention());

        services.AddScoped<IRecruitmentApi, RecruitmentApi>();

        return services;
    }
}
