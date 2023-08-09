namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AvoidUsingMoq : MsBuildProjectFileAnalyzer
{
    public AvoidUsingMoq() : base(Rule.AvoidUsingMoq) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var reference in context.Project.ItemGroups
            .SelectMany(i => i.PackageReferences)
            .Where(IsMoq))
        {
            context.ReportDiagnostic(Descriptor, reference);
        }
    }

    private static bool IsMoq(PackageReference reference)
        => string.Equals(reference.Include, "Moq", StringComparison.OrdinalIgnoreCase)
        && reference.Version is { Length: > 0 } version
        && IsSuspiciousVersion(version);

    private static bool IsSuspiciousVersion(string version)
        => version.Contains("*")
        || (version.Split('.') is { Length: >= 2 } ps
        && int.TryParse(ps[0], out var major)
        && int.TryParse(ps[1], out var minor)
        && (major > 4 || (major == 4 && minor >= 20)));
}
