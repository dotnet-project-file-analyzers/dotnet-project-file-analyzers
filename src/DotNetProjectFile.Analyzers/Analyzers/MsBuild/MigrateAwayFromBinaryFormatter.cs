namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class MigrateAwayFromBinaryFormatter() : MsBuildProjectFileAnalyzer(Rule.MigrateAwayFromBinaryFormatter)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var node in context.File.PropertyGroups
            .Children<EnableUnsafeBinaryFormatterSerialization>(p => p.Val is true))
        {
            context.ReportDiagnostic(Descriptor, node, node.LocalName);
        }
    }
}
