using System.Collections.Immutable;

namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze MS Build project files.
/// </summary>
public abstract class MsBuildProjectFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics) : DiagnosticAnalyzer()
{
    /// <inheritdoc />
    public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [primaryDiagnostic, .. supportedDiagnostics];

    protected virtual IReadOnlyCollection<ProjectFileType> ApplicableTo => ProjectFileTypes.All;

    /// <summary>Gets the primary (first) of the <see cref="SupportedDiagnostics"/>.</summary>
    protected DiagnosticDescriptor Description => SupportedDiagnostics[0];

    /// <inheritdoc />
    public sealed override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        Register(context);
    }

    /// <summary>Registers the analyzer for all MS Build projects files.</summary>
    protected virtual void Register(AnalysisContext context)
        => context.RegisterProjectFileAction(c =>
        {
            if (ApplicableTo.Contains(c.Project.FileType))
            {
                Register(c);
            }
        });

    /// <summary>Registers the analyzer for the <see cref="ProjectFileAnalysisContext"/>.</summary>
    protected abstract void Register(ProjectFileAnalysisContext context);
}
