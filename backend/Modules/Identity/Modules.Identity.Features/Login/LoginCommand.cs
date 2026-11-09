using Modules.Common.Features.Abstractions;
using Modules.Identity.Features.Common;

namespace Modules.Identity.Features.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<AuthResponse>;
