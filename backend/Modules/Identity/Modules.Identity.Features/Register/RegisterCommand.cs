using Modules.Common.Features.Abstractions;
using Modules.Identity.Features.Common;

namespace Modules.Identity.Features.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string FullName,
    string Role) : ICommand<AuthResponse>;
