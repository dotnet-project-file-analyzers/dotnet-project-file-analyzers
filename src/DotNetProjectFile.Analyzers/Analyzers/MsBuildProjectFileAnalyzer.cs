namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze MS Build project files.
/// </summary>
public abstract class MsBuildProjectFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<MsBuildProject>(primaryDiagnostic, supportedDiagnostics)
{
    protected virtual IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.All;

    /// <summary>Registers the analyzer for all MS Build projects files.</summary>
    protected override void Register(AnalysisContext context)
        => context.RegisterProjectFileAction(c =>
        {
            if (ApplicableTo.Contains(c.File.FileType))
            {
                Register(c);
            }
        });
}
