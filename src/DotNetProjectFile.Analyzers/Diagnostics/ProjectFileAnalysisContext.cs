namespace DotNetProjectFile.Diagnostics;

/// <summary>The context required to analyze a MS Build project file.</summary>
public sealed class ProjectFileAnalysisContext
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<Diagnostic> Report;

    /// <summary>Initializes a new instance of the <see cref="ProjectFileAnalysisContext"/> struct.</summary>
    public ProjectFileAnalysisContext(MsBuildProject project, Compilation compilation, AnalyzerOptions options, CancellationToken cancellationToken, Action<Diagnostic> report)
    {
        Project = project;
        Compilation = compilation;
        Options = options;
        CancellationToken = cancellationToken;
        Report = report;

        Config = project.AdditionalText is { } text
            ? options.AnalyzerConfigOptionsProvider.GetOptions(text)
            : MissingAnalyzerConfigOptions.Instance;
    }

    /// <summary>Gets the project file.</summary>
    public MsBuildProject Project { get; }

    /// <summary>Gets the compilation.</summary>
    public Compilation Compilation { get; }

    /// <summary>Gets the analyzer options.</summary>
    public AnalyzerOptions Options { get; }

    /// <summary>Gets the applicable configuration options for the file.</summary>
    public AnalyzerConfigOptions Config { get; }

    /// <summary>Gets the cancellation token.</summary>
    public CancellationToken CancellationToken { get; }

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, Node node, params object?[]? messageArgs)
        => Report(Diagnostic.Create(descriptor, node.Location, messageArgs));
}
