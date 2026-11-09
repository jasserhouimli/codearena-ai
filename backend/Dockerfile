FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy Directory.Build.props and solution file
COPY Directory.Build.props ./
COPY backend.sln ./

# Copy all project files to cache NuGet restore layer
COPY backend.Host/backend.Host.csproj backend.Host/
COPY Modules/Common/Modules.Common.Features/Modules.Common.Features.csproj Modules/Common/Modules.Common.Features/
COPY Modules/Common/Modules.Common.Infrastructure/Modules.Common.Infrastructure.csproj Modules/Common/Modules.Common.Infrastructure/
COPY Modules/Identity/Modules.Identity.Domain/Modules.Identity.Domain.csproj Modules/Identity/Modules.Identity.Domain/
COPY Modules/Identity/Modules.Identity.Features/Modules.Identity.Features.csproj Modules/Identity/Modules.Identity.Features/
COPY Modules/Identity/Modules.Identity.Infrastructure/Modules.Identity.Infrastructure.csproj Modules/Identity/Modules.Identity.Infrastructure/
COPY Modules/Identity/Modules.Identity.PublicApi/Modules.Identity.PublicApi.csproj Modules/Identity/Modules.Identity.PublicApi/
COPY Modules/Recruitment/Modules.Recruitment.Domain/Modules.Recruitment.Domain.csproj Modules/Recruitment/Modules.Recruitment.Domain/
COPY Modules/Recruitment/Modules.Recruitment.Features/Modules.Recruitment.Features.csproj Modules/Recruitment/Modules.Recruitment.Features/
COPY Modules/Recruitment/Modules.Recruitment.Infrastructure/Modules.Recruitment.Infrastructure.csproj Modules/Recruitment/Modules.Recruitment.Infrastructure/
COPY Modules/Recruitment/Modules.Recruitment.PublicApi/Modules.Recruitment.PublicApi.csproj Modules/Recruitment/Modules.Recruitment.PublicApi/
COPY Modules/Assessments/Modules.Assessments.Domain/Modules.Assessments.Domain.csproj Modules/Assessments/Modules.Assessments.Domain/
COPY Modules/Assessments/Modules.Assessments.Features/Modules.Assessments.Features.csproj Modules/Assessments/Modules.Assessments.Features/
COPY Modules/Assessments/Modules.Assessments.Infrastructure/Modules.Assessments.Infrastructure.csproj Modules/Assessments/Modules.Assessments.Infrastructure/
COPY Modules/Assessments/Modules.Assessments.PublicApi/Modules.Assessments.PublicApi.csproj Modules/Assessments/Modules.Assessments.PublicApi/

RUN dotnet restore backend.sln

# Copy the remaining source files and build
COPY . .
WORKDIR /src/backend.Host
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "backend.Host.dll"]
