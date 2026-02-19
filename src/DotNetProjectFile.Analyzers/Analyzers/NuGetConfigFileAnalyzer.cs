using DotNetProjectFile.NuGet.Configuration;

namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze SLNX solution files.
/// </summary>
public abstract class NuGetConfigFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<NuGetConfigFile>(primaryDiagnostic, supportedDiagnostics)
{
    /// <summary>Registers the analyzer for all SLNX solution files.</summary>
    protected override void Register(AnalysisContext context)
        => context.RegisterNuGetConfigFileAction(Register);
}
