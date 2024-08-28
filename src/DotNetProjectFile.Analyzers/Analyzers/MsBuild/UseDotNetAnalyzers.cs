namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseDotNetAnalyzers() : MsBuildProjectFileAnalyzer(Rule.UseDotNetAnalyzers)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.NETAnalyzersEnabled())
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
