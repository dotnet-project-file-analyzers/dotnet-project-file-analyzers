namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineIsPublishable() : MsBuildProjectFileAnalyzer(Rule.DefineIsPublishable)
{
    protected override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.Project.IsTestProject() &&
            context.Project.Property<IsPublishable>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.Project);
        }
    }
}
