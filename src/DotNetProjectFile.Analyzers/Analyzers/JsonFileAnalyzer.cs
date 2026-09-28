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
    /// <summary>
    /// Defines to which <see cref="AnalyzerType"/>s the rule is applicable.
    /// </summary>
    /// <remarks>
    /// Default is <see cref="JsonFileTypes.All"/>.
    /// </remarks>
    public virtual ImmutableArray<AnalyzerType> ApplicableTo => JsonFileTypes.All;

    /// <inheritdoc />
    protected sealed override void Register(AnalysisContext context)
        => context.RegisterJsonFileAction(c =>
        {
            if (ApplicableTo.Contains(c.AnalyzerType))
            {
                Register(c);
            }
        });
}
