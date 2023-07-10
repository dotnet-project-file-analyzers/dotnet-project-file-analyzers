namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddAdditionalFile : MsBuildProjectFileAnalyzer
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
