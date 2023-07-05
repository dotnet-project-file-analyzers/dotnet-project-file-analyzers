namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class ProjectFileCouldNotBeLocated : ProjectFileAnalyzer
{
    public ProjectFileCouldNotBeLocated() : base(Rule.ProjectFileCouldNotBeLocated) { }

    protected override void Register(AnalysisContext context)
        => context.RegisterCompilationAction(Locate);

    private void Locate(CompilationAnalysisContext context)
    {
        var projects = Projects.Init(context);
        if (projects.EntryPoint(context.Compilation.Assembly) is null)
        {
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, Location.None, context.Compilation.SourceModule.ContainingAssembly.Name));
        }
    }

    [ExcludeFromCodeCoverage/* Justification = "Analyzer ensure that this is called for other analyzers." */]
    protected override void Register(ProjectFileAnalysisContext context)
    {
        // Empty.
    }
}
