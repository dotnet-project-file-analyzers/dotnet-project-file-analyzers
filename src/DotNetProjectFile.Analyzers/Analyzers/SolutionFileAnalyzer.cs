using DotNetProjectFile.Slnx;

namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze SLNX solution files.
/// </summary>
public abstract class SolutionFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<Solution>(primaryDiagnostic, supportedDiagnostics)
{
    /// <summary>Registers the analyzer for all RESX resource files.</summary>
    protected override void Register(AnalysisContext context)
        => context.RegisterSolutionFileAction(Register);
}
