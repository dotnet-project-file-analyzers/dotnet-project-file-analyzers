namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RemoveUnusedPackageVersions"/>.</summary>
/// <remarks>
/// This rule is triggered on the <see cref="ProjectFileType.SDK" /> only but
/// analyzes the <see cref="ProjectFileType.DirectoryPackages" />. The reasoning
/// for this is, that it is by far the most conveniant way to ensure that we
/// have all (relevant) MS Build files at our disposal.
/// </remarks>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveUnusedPackageVersions() : MsBuildProjectFileAnalyzer(Rule.RemoveUnusedPackageVersions)
{
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.SDK;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.DirectoryPackagesProps is not { } packages ||
            packages.ManagePackageVersionsCentrally() is not true) { return; }

        var usages = PackageUsages(context);

        foreach (var version in packages.ItemGroups
            .SelectMany(i => i.PackageVersions)
            .Where(i => i.Include is { Length: > 0 } && !usages.Contains(i.Include)))
        {
            context.ReportDiagnostic(Descriptor, version, version.Include);
        }
    }

    private static HashSet<string> PackageUsages(ProjectFileAnalysisContext context)
    {
        HashSet<string> usages = [];

        foreach (var text in context.Options.AdditionalFiles)
        {
            if (ProjectFiles.Global.MsBuildProject(text) is { } project)
            {
                foreach (var reference in project.ItemGroups
                    .SelectMany(i => i.PackageReferences)
                    .Where(r => r.Include is { Length: > 0 })
                    .Select(r => r.Include))
                {
                    usages.Add(reference!);
                }
            }
        }
        return usages;
    }
}
