namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze MS Build project files.
/// </summary>
public abstract class MsBuildProjectFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<MsBuildProject>(primaryDiagnostic, supportedDiagnostics)
{
    /// <summary>
    /// Defines to which <see cref="ProjectFileType"/>s the rule is applicable.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="ProjectFileTypes.All"/>.
    /// </remarks>
    public virtual IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.All;

    /// <summary>Indicates that the rule will not be executed once an import could not be resolved (default is true).</summary>
    public virtual bool DisableOnFailingImport => true;

    /// <summary>Registers the analyzer for all MS Build projects files.</summary>
    protected override void Register(AnalysisContext context)
        => context.RegisterProjectFileAction(c =>
        {
            if (ApplicableTo.Contains(c.File.FileType) && !(c.File.HasFailingImport && DisableOnFailingImport))
            {
                Register(c);
            }
        });
}
