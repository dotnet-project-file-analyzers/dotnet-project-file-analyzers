namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze project files.
/// </summary>
/// <typeparam name="TFile">
/// The type of file to analyze.
/// </typeparam>
public abstract class ProjectFileAnalyzer<TFile>(DiagnosticDescriptor primaryDiagnostic, params DiagnosticDescriptor[] supportedDiagnostics)
    : DiagnosticAnalyzer()
    where TFile : class, ProjectFile
{
    /// <inheritdoc />
    public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [primaryDiagnostic, .. supportedDiagnostics];

    /// <summary>Gets the primary (first) descriptor of the <see cref="SupportedDiagnostics"/>.</summary>
    protected DiagnosticDescriptor Descriptor => SupportedDiagnostics[0];

    /// <inheritdoc />
    public sealed override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        Register(context);
    }

    /// <summary>Registers the analyzer for all projects files.</summary>
    protected abstract void Register(AnalysisContext context);

    /// <summary>Registers the analyzer for the <see cref="ProjectFileAnalysisContext{TFile}"/>.</summary>
    protected abstract void Register(ProjectFileAnalysisContext<TFile> context);
}
