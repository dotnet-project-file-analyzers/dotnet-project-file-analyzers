namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddAdditionalFile() : MsBuildProjectFileAnalyzer(Rule.AddAdditionalFile)
{
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.IsAdditional && !context.Project.IsDirectoryBuildProps)
        {
            context.ReportDiagnostic(Descriptor, context.Project, context.Project.Path.Name);
        }
    }
}
