using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Modules.Identity.Domain.Constants;
using Modules.Identity.Domain.Entities;

namespace Modules.Identity.Infrastructure.Database;

public static class IdentityDataSeeder
{
    public static async Task SeedRolesAsync(RoleManager<Role> roleManager, ILogger logger)
    {
        string[] roles = [Roles.Admin, Roles.Recruiter, Roles.Candidate];

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var role = new Role(roleName, $"Default {roleName} role");
                var result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    logger.LogInformation("Seeded role: {Role}", roleName);
                }
                else
                {
                    logger.LogError("Failed to seed role: {Role}", roleName);
                }
            }
        }
    }
}
