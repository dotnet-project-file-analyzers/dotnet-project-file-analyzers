using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Diagnostics;

/// <summary>The context required to analyze a project file.</summary>
/// <typeparam name="TFile">
/// The type of file to analyze.
/// </typeparam>
public readonly struct ProjectFileAnalysisContext<TFile>(
    TFile file,
    Compilation compilation,
    AnalyzerOptions options,
    CancellationToken cancellationToken,
    Action<Diagnostic> report)
    where TFile : ProjectFile
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<Diagnostic> Report = report;

    /// <summary>Gets the project file.</summary>
    public TFile File { get; } = file;

    /// <summary>Gets the compilation language.</summary>
    public Language CompilationLanguage => Language.Parse(Compilation.Language);

    /// <summary>Gets the compilation.</summary>
    public Compilation Compilation { get; } = compilation;

    /// <summary>Gets the analyzer options.</summary>
    public AnalyzerOptions Options { get; } = options;

    /// <summary>Gets the MS Build property.</summary>
    [Pure]
    public string? GetMsBuildProperty(string propertyName) => Options.GetMsBuildProperty(propertyName);

    /// <summary>Gets the cancellation token.</summary>
    public CancellationToken CancellationToken { get; } = cancellationToken;

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, XmlAnalysisNode node, params object?[]? messageArgs)
        => ReportDiagnostic(
            descriptor,
            node.Element.Parent is null ? node.Locations.StartElement : node.Locations.FullSpan,
            messageArgs);

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, ProjectFile file, LinePositionSpan span, params object?[]? messageArgs)
        => ReportDiagnostic(descriptor, file.GetLocation(span), messageArgs);

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, Location location, params object?[]? messageArgs)
        => ReportDiagnostic(Diagnostic.Create(descriptor, location, messageArgs));

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(Diagnostic diagnostic)
    {
        var warningPragmas = File.WarningPragmas;

        if (!warningPragmas.IsDisabled(diagnostic.Descriptor.Id, diagnostic.Location))
        {
            Report(diagnostic);
        }
    }
}
