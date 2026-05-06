namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RemoveRedundantReferencesForGloballyReferenced" />.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveRedundantReferencesForGloballyReferenced() : MsBuildProjectFileAnalyzer(Rule.RemoveRedundantReferencesForGloballyReferenced)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.EnabledItems<GlobalPackageReference>()
            .Where(r => r.Include is { Length: > 0 })
            .Select(r => r.Include)
            .ToImmutableHashSet(StringComparer.OrdinalIgnoreCase) is not { Count: > 0 } globals) return;

        foreach (var reference in context.EnabledItems<PackageReference>()
            .Where(r => globals.Contains(r.Include)))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.Include);
        }
    }
}
