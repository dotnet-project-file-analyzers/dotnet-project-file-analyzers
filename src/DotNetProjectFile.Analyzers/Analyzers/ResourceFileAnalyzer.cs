using DotNetProjectFile.Resx;

namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze RESX resource files.
/// </summary>
public abstract class ResourceFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<Resource>(primaryDiagnostic, supportedDiagnostics)
{
    /// <summary>Registers the analyzer for all RESX resource files.</summary>
    protected override void Register(AnalysisContext context)
        => context.RegisterResourceFileAction(Register);
}
