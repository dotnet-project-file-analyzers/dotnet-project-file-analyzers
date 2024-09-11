namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class AddAdditionalFile() : MsBuildProjectFileAnalyzer(Rule.AddAdditionalFile)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile_Props;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.IsAdditional)
        {
            context.ReportDiagnostic(Description, context.Project, context.Project.Path.Name);
        }
    }
}
