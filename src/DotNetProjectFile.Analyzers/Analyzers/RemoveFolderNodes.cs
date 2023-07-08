namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RemoveFolderNodes : ProjectFileAnalyzer
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
