namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineOutputType : MsBuildProjectFileAnalyzer
{
    public DefineOutputType() : base(Rule.DefineOutputType) { }

    protected override bool ApplyToProps => false;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.Project
            .ImportsAndSelf()
            .SelectMany(p => p.PropertyGroups)
            .SelectMany(g => g.OutputType).None())
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
