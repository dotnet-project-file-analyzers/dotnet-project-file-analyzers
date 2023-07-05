namespace DotNetProjectFile.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GuardUnsupported : ProjectFileAnalyzer
{
    public GuardUnsupported() : base(
        Rule.ProjectFileCouldNotBeLocated,
        Rule.UpdateLegacyProjects) { }

    protected override void Register(AnalysisContext context)
        => context.RegisterCompilationAction(Locate);

    private void Locate(CompilationAnalysisContext context)
    {
        var projects = Projects.Init(context);
        if (projects.EntryPoint(context.Compilation.Assembly) is not { } project)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Rule.ProjectFileCouldNotBeLocated,
                    Location.None,
                    context.Compilation.SourceModule.ContainingAssembly.Name));
        }
        else if (project.Element.Name.Namespace is { } ns && !string.IsNullOrEmpty(ns.NamespaceName))
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule.UpdateLegacyProjects, project.Location));
        }
    }

    [ExcludeFromCodeCoverage/* Justification = "Analyzer ensure that this is called for other analyzers." */]
    protected override void Register(ProjectFileAnalysisContext context)
    {
        // Empty.
    }
}
