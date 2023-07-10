namespace DotNetProjectFile.Diagnostics;

/// <summary>The context required to analyze a MS Build project file.</summary>
public readonly struct ProjectFileAnalysisContext
{
    /// <summary>Gets the project file.</summary>
    public readonly MsBuild.Project Project;

    /// <summary>Gets the compilation.</summary>
    public readonly Compilation Compilation;

    /// <summary>Gets the analyzer options.</summary>
    public readonly AnalyzerOptions Options;

    /// <summary>Gets the cancellation token.</summary>
    public readonly CancellationToken CancellationToken;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<Diagnostic> Report;

    /// <summary>Initializes a new instance of the <see cref="ProjectFileAnalysisContext"/> struct.</summary>
    public ProjectFileAnalysisContext(MsBuild.Project project, Compilation compilation, AnalyzerOptions options, CancellationToken cancellationToken, Action<Diagnostic> report)
    {
        Project = project;
        Compilation = compilation;
        Options = options;
        CancellationToken = cancellationToken;
        Report = report;
    }

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, Node node, params object?[]? messageArgs)
        => Report(Diagnostic.Create(descriptor, node.Location, messageArgs));
}
