using System.Collections.Immutable;

namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefinePackageReferenceVersion()
    : MsBuildProjectFileAnalyzer(Rule.DefinePackageReferenceVersion)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        var versions = context.Project.ManagePackageVersionsCentrally() is true
            ? PackageVersions(context.Project)
            : [];

        var references = context.Project.ItemGroups
            .SelectMany(g => g.PackageReferences)
            .Where(r => r.IncludeOrUpdate is { Length: > 0 })
            .Where(r => r.VersionOrVersionOverride is not { Length: > 0 });

        foreach (var reference in references)
        {
            if (!versions.Contains(reference.IncludeOrUpdate))
            {
                context.ReportDiagnostic(Description, reference, reference.IncludeOrUpdate);
            }
        }
    }

    private static ImmutableHashSet<string> PackageVersions(MsBuildProject project) => project
        .ImportsAndSelf()
        .SelectMany(p => p.ItemGroups)
        .SelectMany(g => g.PackageVersions)
        .Where(r => r.Version is { Length: > 0 })
        .Where(r => r.IncludeOrUpdate is { Length: > 0 })
        .Select(r => r.IncludeOrUpdate)
        .ToImmutableHashSet();
}
