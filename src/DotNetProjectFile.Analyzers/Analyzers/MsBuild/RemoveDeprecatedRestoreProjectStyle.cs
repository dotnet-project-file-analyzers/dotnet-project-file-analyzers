namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RemoveDeprecatedRestoreProjectStyle"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveDeprecatedRestoreProjectStyle() : MsBuildProjectFileAnalyzer(Rule.RemoveDeprecatedRestoreProjectStyle)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var prop in context.File.PropertyGroups.Children<RestoreProjectStyle>())
        {
            context.ReportDiagnostic(Descriptor, prop);
        }
    }
}
