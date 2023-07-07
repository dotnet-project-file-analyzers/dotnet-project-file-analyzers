namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddAdditionalFile : ProjectFileAnalyzer
{
    public AddAdditionalFile() : base(Rule.AddAdditionalFile) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var project in context.Project.AncestorsAndSelf().Where(p => !p.IsAdditional))
        {
            context.ReportDiagnostic(Descriptor, project.Location, project.Path.Name);
        }
    }
}
