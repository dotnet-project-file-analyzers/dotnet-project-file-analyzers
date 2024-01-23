namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineIsPackable() : MsBuildProjectFileAnalyzer(Rule.DefineIsPackable)
{
    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.IsPackable).None())
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
