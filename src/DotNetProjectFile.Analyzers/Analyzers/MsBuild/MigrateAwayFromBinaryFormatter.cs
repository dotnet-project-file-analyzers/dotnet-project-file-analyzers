namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class MigrateAwayFromBinaryFormatter() : MsBuildProjectFileAnalyzer(Rule.MigrateAwayFromBinaryFormatter)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        foreach(var node in context.File.PropertyGroups
            .SelectMany(g => g.EnableUnsafeBinaryFormatterSerialization)
            .Where(p => p.Value is true))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }
    }
}
