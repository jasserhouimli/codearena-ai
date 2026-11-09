using Microsoft.AspNetCore.Identity;

namespace Modules.Identity.Domain.Entities;

public class Role : IdentityRole<Guid>
{
    public string? Description { get; set; }

    public Role() : base()
    {
    }

    public Role(string roleName, string? description = null) : base(roleName)
    {
        Description = description;
    }
}
