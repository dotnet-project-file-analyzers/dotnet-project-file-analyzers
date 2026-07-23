namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class DefineIsPublishable() : MsBuildProjectFileAnalyzer(Rule.DefineIsPublishable)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.IsTestProject() &&
            context.File.Property<IsPublishable>() is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
