namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.VersionSuffixShouldBeSemVerCompliant"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class VersionSuffixShouldBeSemVerCompliant() : MsBuildProjectFileAnalyzer(Rule.VersionSuffixShouldBeSemVerCompliant)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var version in context.File.PropertyGroups.Children<VersionSuffix>(NotSemantic))
        {
            context.ReportDiagnostic(Descriptor, version, version.Element.Value);
        }
    }

    private static bool NotSemantic(VersionSuffix version)
        => version.Value is not { } val
        || SemVer.TryParse($"1.2.3-{val}") is not null
        || SemVer.TryParse($"1.2.3+{val}") is not null;
}
