namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Impelements <see cref="Rule.UseDotNetAnalyzers"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class UseDotNetAnalyzers() : MsBuildProjectFileAnalyzer(Rule.UseDotNetAnalyzers)
{
    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    public override ImmutableArray<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        if (!context.EnableNETAnalyzers)
        {
            context.ReportDiagnostic(Descriptor, context.File);
        }
    }
}
