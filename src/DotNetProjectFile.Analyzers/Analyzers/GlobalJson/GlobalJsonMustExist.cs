namespace DotNetProjectFile.Analyzers.GlobalJson;

/// <summary>Implements <see cref="Rule.Json.GlobalJsonMustExist"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class GlobalJsonMustExist() : MsBuildProjectFileAnalyzer(Rule.Json.GlobalJsonMustExist)
{
    /// <inheritdoc />
    public override ImmutableArray<AnalyzerType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext<MsBuildProject> context)
    {
        if (context.File.GlobalJson is null)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
