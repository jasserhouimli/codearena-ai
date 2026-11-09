using Modules.Common.Features.Abstractions;
using Modules.Identity.Features.Common;

namespace Modules.Identity.Features.GitHubOAuth;

public sealed record GitHubOAuthCommand(string Code) : ICommand<AuthResponse>;
