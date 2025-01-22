using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers;

public abstract class IniFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<OldIniFile>(primaryDiagnostic, supportedDiagnostics)
{
    /// <inheritdoc />
    protected sealed override void Register(AnalysisContext context)
        => context.RegisterEditorConfigFileAction(Register);
}
