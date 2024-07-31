namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseDotNetAnalyzers() : MsBuildProjectFileAnalyzer(Rule.UseDotNetAnalyzers)
{
    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.NETAnalyzersEnabled())
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
