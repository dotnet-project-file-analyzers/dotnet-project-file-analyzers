namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class StaticAliasUsingNotSupported() : MsBuildProjectFileAnalyzer(Rule.StaticAliasUsingNotSupported)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var directive in context.File.ItemGroups
            .Children<Using>(u => u.Type == UsingType.StaticAlias))
        {
            context.ReportDiagnostic(Descriptor, directive, directive.Include);
        }
    }
}
