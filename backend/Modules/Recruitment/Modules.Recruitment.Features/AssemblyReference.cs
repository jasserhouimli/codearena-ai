using System.Reflection;

namespace Modules.Recruitment.Features;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
