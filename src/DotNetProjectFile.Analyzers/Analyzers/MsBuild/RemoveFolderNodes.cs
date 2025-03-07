namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveFolderNodes() : MsBuildProjectFileAnalyzer(Rule.RemoveFolderNodes)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var folder in context.File.ItemGroups.SelectMany(p => p.Folders))
        {
            context.ReportDiagnostic(Descriptor, folder, folder.Include?.TrimEnd('/', '\\'));
        }
    }
}
