namespace DotNetProjectFile.Diagnostics;

/// <summary>The context required to analyze a MS Build project file.</summary>
public sealed class ProjectFileAnalysisContext(
    MsBuildProject project,
    Compilation compilation,
    AnalyzerOptions options,
    CancellationToken cancellationToken,
    Action<Diagnostic> report)
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<Diagnostic> Report = report;

    /// <summary>Gets the project file.</summary>
    public MsBuildProject Project { get; } = project;

    /// <summary>Gets the compilation.</summary>
    public Compilation Compilation { get; } = compilation;

    /// <summary>Gets the analyzer options.</summary>
    public AnalyzerOptions Options { get; } = options;

    /// <summary>Gets the applicable configuration options for the file.</summary>
    public AnalyzerConfigOptions Config { get; } = project.AdditionalText is { } text
        ? options.AnalyzerConfigOptionsProvider.GetOptions(text)
        : MissingAnalyzerConfigOptions.Instance;

    /// <summary>Gets the cancellation token.</summary>
    public CancellationToken CancellationToken { get; } = cancellationToken;

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, Node node, params object?[]? messageArgs)
        => Report(Diagnostic.Create(descriptor, node.Location, messageArgs));
}
