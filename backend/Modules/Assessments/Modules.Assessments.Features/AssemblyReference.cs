using System.Reflection;

namespace Modules.Assessments.Features;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
