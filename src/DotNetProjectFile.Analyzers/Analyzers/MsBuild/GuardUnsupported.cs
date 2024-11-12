namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GuardUnsupported() : MsBuildProjectFileAnalyzer(
    Rule.ProjectFileCouldNotBeLocated,
    Rule.UpdateLegacyProjects)
{
    protected override void Register(AnalysisContext context)
        => context.RegisterCompilationAction(Locate);

    private static void Locate(CompilationAnalysisContext context)
    {
        if (ProjectFiles.Global.UpdateMsBuildProject(context) is not { } project)
        {
            context.ReportDiagnostic(
                Diagnostic.Create(
                    Rule.ProjectFileCouldNotBeLocated,
                    Location.None,
                    context.Compilation.SourceModule.ContainingAssembly.Name));
        }
        else if (project.IsLegacy)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule.UpdateLegacyProjects, project.GetLocation(project.Positions.FullSpan)));
        }
    }

    [ExcludeFromCodeCoverage/* Justification = "Analyzer ensure that this is called for other analyzers." */]
    protected override void Register(ProjectFileAnalysisContext context)
    {
        // Empty.
    }
}
