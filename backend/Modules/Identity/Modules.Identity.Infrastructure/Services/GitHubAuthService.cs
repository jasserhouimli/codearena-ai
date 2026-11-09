using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Modules.Identity.Infrastructure.Services;

public class GitHubAuthService : IGitHubAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<GitHubAuthService> _logger;

    public GitHubAuthService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<GitHubAuthService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<GitHubUserProfile?> ExchangeCodeForProfileAsync(string code, CancellationToken cancellationToken = default)
    {
        string clientId = _configuration["GitHub:ClientId"] ?? string.Empty;
        string clientSecret = _configuration["GitHub:ClientSecret"] ?? string.Empty;

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
        {
            _logger.LogWarning("GitHub ClientId or ClientSecret is not configured.");
        }

        // 1. Exchange OAuth code for GitHub Access Token
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token");
        tokenRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "code", code }
        });

        var tokenResponse = await _httpClient.SendAsync(tokenRequest, cancellationToken);
        if (!tokenResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to exchange GitHub OAuth code. Status: {Status}", tokenResponse.StatusCode);
            return null;
        }

        var tokenContent = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);
        var tokenResult = JsonSerializer.Deserialize<GitHubTokenResponse>(tokenContent);

        if (string.IsNullOrEmpty(tokenResult?.AccessToken))
        {
            _logger.LogError("GitHub OAuth response did not contain an access token: {Content}", tokenContent);
            return null;
        }

        string accessToken = tokenResult.AccessToken;

        // 2. Fetch User Profile from GitHub API
        var userRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
        userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        userRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("CodeArena-AI", "1.0"));

        var userResponse = await _httpClient.SendAsync(userRequest, cancellationToken);
        if (!userResponse.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to fetch GitHub user profile. Status: {Status}", userResponse.StatusCode);
            return null;
        }

        var userContent = await userResponse.Content.ReadAsStringAsync(cancellationToken);
        var gitHubUser = JsonSerializer.Deserialize<GitHubApiUserResponse>(userContent);

        if (gitHubUser is null)
        {
            return null;
        }

        string email = gitHubUser.Email ?? string.Empty;

        // 3. If email is null/private, fetch emails endpoint
        if (string.IsNullOrEmpty(email))
        {
            var emailsRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
            emailsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            emailsRequest.Headers.UserAgent.Add(new ProductInfoHeaderValue("CodeArena-AI", "1.0"));

            var emailsResponse = await _httpClient.SendAsync(emailsRequest, cancellationToken);
            if (emailsResponse.IsSuccessStatusCode)
            {
                var emailsContent = await emailsResponse.Content.ReadAsStringAsync(cancellationToken);
                var emailsList = JsonSerializer.Deserialize<List<GitHubEmailResponse>>(emailsContent);
                var primaryEmail = emailsList?.FirstOrDefault(e => e.Primary && e.Verified)?.Email
                                  ?? emailsList?.FirstOrDefault()?.Email;

                if (!string.IsNullOrEmpty(primaryEmail))
                {
                    email = primaryEmail;
                }
            }
        }

        // Fallback email if GitHub has no public or private accessible email
        if (string.IsNullOrEmpty(email))
        {
            email = $"{gitHubUser.Login}@users.noreply.github.com";
        }

        return new GitHubUserProfile(
            gitHubUser.Id.ToString(),
            gitHubUser.Login,
            string.IsNullOrWhiteSpace(gitHubUser.Name) ? gitHubUser.Login : gitHubUser.Name,
            email,
            gitHubUser.AvatarUrl,
            gitHubUser.Bio,
            accessToken);
    }

    private sealed class GitHubTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string? TokenType { get; set; }

        [JsonPropertyName("scope")]
        public string? Scope { get; set; }
    }

    private sealed class GitHubApiUserResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("avatar_url")]
        public string? AvatarUrl { get; set; }

        [JsonPropertyName("bio")]
        public string? Bio { get; set; }
    }

    private sealed class GitHubEmailResponse
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("primary")]
        public bool Primary { get; set; }

        [JsonPropertyName("verified")]
        public bool Verified { get; set; }
    }
}
