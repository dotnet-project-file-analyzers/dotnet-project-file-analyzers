namespace DotNetProjectFile.Analyzers.MsBuild;

/// <summary>Implements <see cref="Rule.LogAdditionalFile"/>.</summary>
[DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
public sealed class LogAdditionalFiles() : MsBuildProjectFileAnalyzer(Rule.LogAdditionalFile)
{
    /// <inheritdoc />
    public override IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.ProjectFile;

    /// <inheritdoc />
    public override bool DisableOnFailingImport => false;

    /// <inheritdoc />
    protected override void Register(ProjectFileAnalysisContext context)
    {
        foreach (var file in context.Options.AdditionalFiles)
        {
            context.ReportDiagnostic(Descriptor, context.File, file.Path);
        }
    }
}
