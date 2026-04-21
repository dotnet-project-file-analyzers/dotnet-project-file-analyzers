namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.VersionPrefixShouldBeSemVerCompliant"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class VersionPrefixShouldBeSemVerCompliant() : MsBuildProjectFileAnalyzer(Rule.VersionPrefixShouldBeSemVerCompliant)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var version in context.File.PropertyGroups.Children<VersionPrefix>(c => !IsSemantic(c)))
        {
            context.ReportDiagnostic(Descriptor, version, version.Element.Value);
        }
    }

    private static bool IsSemantic(VersionPrefix version)
        => version.Value is { } sv
        && sv.PreRelease is null
        && sv.BuildMetadata is null;
}
