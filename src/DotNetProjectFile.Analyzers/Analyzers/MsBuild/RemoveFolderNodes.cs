namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveFolderNodes() : MsBuildProjectFileAnalyzer(Rule.RemoveFolderNodes)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var folder in context.Project.ItemGroups.SelectMany(p => p.Folders))
        {
            context.ReportDiagnostic(Descriptor, folder, folder.Include?.TrimEnd('/', '\\'));
        }
    }
}
