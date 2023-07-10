namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddAdditionalFile : MsBuildProjectFileAnalyzer
{
    public AddAdditionalFile() : base(Rule.AddAdditionalFile) { }

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.IsAdditional)
        {
            context.ReportDiagnostic(Descriptor, context.Project, context.Project.Path.Name);
        }
    }
}
