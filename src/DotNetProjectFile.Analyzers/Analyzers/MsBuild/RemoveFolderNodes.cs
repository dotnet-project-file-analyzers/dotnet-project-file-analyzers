namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveFolderNodes : MsBuildProjectFileAnalyzer
{
    public RemoveFolderNodes() : base(Rule.RemoveFolderNodes) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var folder in context.Project
             .AncestorsAndSelf()
             .SelectMany(p => p.ItemGroups)
             .SelectMany(p => p.Folders))
        {
            context.ReportDiagnostic(Descriptor, folder.Location, folder.Include?.TrimEnd('/', '\\'));
        }
    }
}
