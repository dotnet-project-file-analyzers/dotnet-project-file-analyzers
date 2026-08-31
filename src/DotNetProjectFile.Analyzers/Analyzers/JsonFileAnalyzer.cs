using DotNetProjectFile.Json;

namespace DotNetProjectFile.Analyzers;

/// <summary>
/// Base for <see cref="DiagnosticAnalyzer"/>s to analyze JSON solution files.
/// </summary>
public abstract class JsonFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<JsonFile>(primaryDiagnostic, supportedDiagnostics)
{
    /// <summary>Registers the analyzer for all SLNX solution files.</summary>
    /// <inheritdoc />
    protected override void Register(AnalysisContext context)
        => context.RegisterJsonFileAction(Register);
}
