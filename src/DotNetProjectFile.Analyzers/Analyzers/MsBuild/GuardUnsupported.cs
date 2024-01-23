namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GuardUnsupported() : MsBuildProjectFileAnalyzer(
    Rule.ProjectFileCouldNotBeLocated,
    Rule.UpdateLegacyProjects)
{
    protected override void Register(AnalysisContext context)
        => context.RegisterCompilationAction(Locate);

    private void Locate(CompilationAnalysisContext context)
    {
        if (Projects.Init(context).EntryPoint(context) is not { } project)
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
