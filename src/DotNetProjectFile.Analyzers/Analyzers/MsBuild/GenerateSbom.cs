namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GenerateSbom() : MsBuildProjectFileAnalyzer(Rule.GenerateSbom)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (context.File.IsPackable() && context.File.Property<GenerateSBOM>()?.Value is not true)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
