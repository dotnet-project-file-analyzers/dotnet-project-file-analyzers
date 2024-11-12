namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidUsingMoq() : MsBuildProjectFileAnalyzer(Rule.AvoidUsingMoq)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var reported = false;

        foreach (var reference in context.File.ItemGroups
            .SelectMany(i => i.PackageReferences)
            .Where(IsMoq))
        {
            context.ReportDiagnostic(Descriptor, reference);
            reported = true;
        }

        if (!reported && context.Compilation.ReferencedAssemblyNames.Any(IsMoq))
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }

    private static bool IsMoq(PackageReference reference)
        => IsMoq(reference.Include)
        && reference.VersionOrVersionOverride is { Length: > 0 } version
        && IsSuspiciousVersion(version);

    private static bool IsMoq(AssemblyIdentity assembly)
        => IsMoq(assembly.Name)
        && assembly.Version >= new System.Version(4, 20);

    private static bool IsMoq(string? name) => name.IsMatch("Moq");

    private static bool IsSuspiciousVersion(string version)
        => version.Contains("*")
        || (version.Split('.') is { Length: >= 2 } ps
        && int.TryParse(ps[0], out var major)
        && int.TryParse(ps[1], out var minor)
        && (major > 4 || (major == 4 && minor >= 20)));
}
