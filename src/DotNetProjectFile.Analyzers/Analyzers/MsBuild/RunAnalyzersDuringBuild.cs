namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.RunAnalyzersDuringBuild" />.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class RunAnalyzersDuringBuild() : MsBuildProjectFileAnalyzer(Rule.RunAnalyzersDuringBuild)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile_SDK;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var property in context.File
            .Properties<DotNetProjectFile.MsBuild.RunAnalyzersDuringBuild>()
            .Where(p => p.Value is false))
        {
            context.ReportDiagnostic(Descriptor, property);
        }
    }
}
