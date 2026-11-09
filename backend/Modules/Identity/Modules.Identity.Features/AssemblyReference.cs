using System.Reflection;

namespace Modules.Identity.Features;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
