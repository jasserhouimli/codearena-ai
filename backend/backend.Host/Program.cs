using backend.Host;
using backend.Host.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

// Global RFC 7807 Exception Handler
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

// Automatically map all vertical slice endpoints registered from all modules
app.MapEndpoints();

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }))
   .WithTags("HealthCheck");

await app.RunAsync();

public partial class Program;
