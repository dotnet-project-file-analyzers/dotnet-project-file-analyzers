using System.Collections.Immutable;

namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze RESX resource files.
/// </summary>
public abstract class ResourceFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics) : DiagnosticAnalyzer
{
    /// <inheritdoc />
    public sealed override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } = [primaryDiagnostic, .. supportedDiagnostics];

    /// <summary>Gets the primary (first) of the <see cref="SupportedDiagnostics"/>.</summary>
    public DiagnosticDescriptor Description => SupportedDiagnostics[0];

    /// <inheritdoc />
    public sealed override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        Register(context);
    }

    /// <summary>Registers the analyzer for all RESX resource files.</summary>
    protected virtual void Register(AnalysisContext context)
        => context.RegisterResourceFileAction(Register);

    /// <summary>Registers the analyzer for the <see cref="ResourceFileAnalysisContext"/>.</summary>
    protected abstract void Register(ResourceFileAnalysisContext context);
}
