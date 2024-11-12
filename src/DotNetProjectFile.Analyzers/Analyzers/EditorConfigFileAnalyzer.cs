using DotNetProjectFile.EditorConfig;

namespace DotNetProjectFile.Analyzers;

public abstract class EditorConfigFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<EditorConfigFile>(primaryDiagnostic, supportedDiagnostics)
{
    /// <inheritdoc />
    protected sealed override void Register(AnalysisContext context)
        => context.RegisterEditorConfigFileAction(Register);
}
