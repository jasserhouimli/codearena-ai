using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using backend.Host.Extensions;
using backend.Host.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Modules.Assessments.Features;
using Modules.Assessments.Infrastructure;
using Modules.Common.Features;
using Modules.Common.Infrastructure;
using Modules.Identity.Features;
using Modules.Identity.Infrastructure;
using Modules.Recruitment.Features;
using Modules.Recruitment.Infrastructure;

namespace backend.Host;

internal static class DependencyInjection
{
    public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        string frontendUrl = builder.Configuration["FrontendUrl"] ?? "http://localhost:5173";

        builder.Services.AddCors(options =>
            options.AddPolicy("AllowFrontend", policy =>
                policy
                    .WithOrigins(frontendUrl)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
            )
        );

        return builder;
    }

    public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "CodeArena AI API",
                Version = "v1",
                Description = "Intelligent Multi-Stage Technical Recruitment & Code Defense Platform API"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Format: 'Bearer {token}'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return builder;
    }

    public static WebApplicationBuilder AddErrorHandling(this WebApplicationBuilder builder)
    {
        builder.Services.AddProblemDetails(options =>
            options.CustomizeProblemDetails = context =>
                context.ProblemDetails.Extensions.TryAdd("traceId", context.HttpContext.TraceIdentifier)
        );

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        return builder;
    }

    public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
    {
        string jwtKey = builder.Configuration["Jwt:Key"] ?? "super_secret_codearena_jwt_key_that_is_at_least_32_bytes_long!";
        string issuer = builder.Configuration["Jwt:Issuer"] ?? "CodeArena";
        string audience = builder.Configuration["Jwt:Audience"] ?? "CodeArenaCandidates";

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
            });

        builder.Services.AddAuthorization();

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        Assembly[] moduleAssemblies =
        [
            AssemblyReference.Assembly,
            Modules.Identity.Features.AssemblyReference.Assembly,
            Modules.Recruitment.Features.AssemblyReference.Assembly,
            Modules.Assessments.Features.AssemblyReference.Assembly
        ];

        // Register CQRS command/query handlers and FluentValidation validators
        builder.Services.AddCommonModule(moduleAssemblies);

        // Register vertical slice Minimal API endpoints
        builder.Services.AddEndpoints(moduleAssemblies);

        // Module services & Infrastructure
        builder.Services.AddIdentityModule()
            .AddIdentityInfrastructure(builder.Configuration);

        builder.Services.AddRecruitmentModule()
            .AddRecruitmentInfrastructure(builder.Configuration);

        builder.Services.AddAssessmentsModule()
            .AddAssessmentsInfrastructure(builder.Configuration);

        builder.Services.AddCommonInfrastructure(builder.Configuration);

        return builder;
    }
}
