namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineIsPublishable() : MsBuildProjectFileAnalyzer(Rule.DefineIsPublishable)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsTestProject() &&
            context.File.Property<IsPublishable>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
