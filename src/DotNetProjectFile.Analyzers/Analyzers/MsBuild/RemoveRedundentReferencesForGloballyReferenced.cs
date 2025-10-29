namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RemoveRedundentReferencesForGloballyReferenced" />.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveRedundentReferencesForGloballyReferenced() : MsBuildProjectFileAnalyzer(Rule.RemoveRedundentReferencesForGloballyReferenced)
{
    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.Walk()
            .OfType<GlobalPackageReference>()
            .Where(r => r.Include is { Length: > 0 })
            .Select(r => r.Include)
            .ToImmutableHashSet(StringComparer.OrdinalIgnoreCase) is not { Count: > 0 } globals) return;

        foreach (var reference in context.File.Walk()
            .OfType<PackageReference>()
            .Where(r => globals.Contains(r.Include)))
        {
            context.ReportDiagnostic(Descriptor, reference, reference.Include);
        }
    }
}
