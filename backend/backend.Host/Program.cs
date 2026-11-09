using backend.Host;
using backend.Host.Extensions;
using Microsoft.AspNetCore.Identity;
using Modules.Identity.Domain.Entities;
using Modules.Identity.Infrastructure.Database;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder
    .AddApi()
    .AddSwagger()
    .AddErrorHandling()
    .AddAuthentication()
    .AddServices();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CodeArena AI v1");
    });
}

// Automatically apply multi-DbContext migrations across Supabase PostgreSQL
await app.ApplyMigrationsAsync();

// Seed initial roles (Admin, Recruiter, Candidate)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    await IdentityDataSeeder.SeedRolesAsync(roleManager, logger);
}

// Global RFC 7807 Problem Details
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Automatically map all vertical slice endpoints registered across all modules
app.MapEndpoints();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }))
   .WithTags("HealthCheck");

await app.RunAsync();

public partial class Program;
