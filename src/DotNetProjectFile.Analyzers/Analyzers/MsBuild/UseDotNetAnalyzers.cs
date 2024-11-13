namespace DotNetProjectFile.Analyzers.MsBuild;

[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseDotNetAnalyzers() : MsBuildProjectFileAnalyzer(Rule.UseDotNetAnalyzers)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.File.NETAnalyzersEnabled())
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
