using DotNetProjectFile.Ini;

namespace DotNetProjectFile.Analyzers;

public abstract class IniFileAnalyzer(
    DiagnosticDescriptor primaryDiagnostic,
    params DiagnosticDescriptor[] supportedDiagnostics)
    : ProjectFileAnalyzer<IniFile>(primaryDiagnostic, supportedDiagnostics)
{
    /// <summary>
    /// Defines to which <see cref="AnalyzerType"/>s the rule is applicable.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="IniFileTypes.All"/>.
    /// </remarks>
    public virtual ImmutableArray<AnalyzerType> ApplicableTo => IniFileTypes.All;

    /// <inheritdoc />
    protected override void Register(AnalysisContext context)
        => context.RegisterEditorConfigFileAction(c =>
        {
            if (ApplicableTo.Contains(c.AnalyzerType))
            {
                Register(c);
            }
        });
}
