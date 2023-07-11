using DotNetProjectFile.CodeAnalysis;

namespace DotNetProjectFile.Diagnostics;

/// <summary>The context required to analyze a MS Build project file.</summary>
public readonly struct ResourceFileAnalysisContext
{
    /// <summary>Gets the project file.</summary>
    public readonly Resx.Resource Resource;

    /// <summary>Gets the compilation.</summary>
    public readonly Compilation Compilation;

    /// <summary>Gets the analyzer options.</summary>
    public readonly AnalyzerOptions Options;

    /// <summary>Gets the cancellation token.</summary>
    public readonly CancellationToken CancellationToken;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<Diagnostic> Report;

    /// <summary>Initializes a new instance of the <see cref="ResourceFileAnalysisContext"/> struct.</summary>
    public ResourceFileAnalysisContext(Resx.Resource resource, Compilation compilation, AnalyzerOptions options, CancellationToken cancellationToken, Action<Diagnostic> report)
    {
        Resource = resource;
        Compilation = compilation;
        Options = options;
        CancellationToken = cancellationToken;
        Report = report;
    }

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, Locatable location, params object?[]? messageArgs)
        => Report(Diagnostic.Create(descriptor, location.Location, messageArgs));
}
