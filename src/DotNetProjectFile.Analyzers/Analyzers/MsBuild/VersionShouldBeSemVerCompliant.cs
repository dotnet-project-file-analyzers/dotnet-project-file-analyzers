using Version = DotNetProjectFile.MsBuild.Version;

namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.VersionShouldBeSemVerCompliant"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class VersionShouldBeSemVerCompliant() : MsBuildProjectFileAnalyzer(Rule.VersionShouldBeSemVerCompliant)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var version in context.File.PropertyGroups.Children<Version>(NotSemantic))
        {
            context.ReportDiagnostic(Descriptor, version, version.Element.Value);
        }
    }

    private static bool NotSemantic(Version version) => version is { Value: null };
}
