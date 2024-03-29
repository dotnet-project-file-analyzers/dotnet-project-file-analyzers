﻿using DotNetProjectFile.CodeAnalysis;

namespace DotNetProjectFile.Diagnostics;

/// <summary>The context required to analyze a MS Build project file.</summary>
/// <remarks>Initializes a new instance of the <see cref="ResourceFileAnalysisContext"/> struct.</remarks>
public readonly struct ResourceFileAnalysisContext(
    Resx.Resource resource,
    Compilation compilation,
    AnalyzerOptions options,
    CancellationToken cancellationToken,
    Action<Diagnostic> report)
{
    /// <summary>Gets the project file.</summary>
    public readonly Resx.Resource Resource = resource;

    /// <summary>Gets the compilation.</summary>
    public readonly Compilation Compilation = compilation;

    /// <summary>Gets the analyzer options.</summary>
    public readonly AnalyzerOptions Options = options;

    /// <summary>Gets the cancellation token.</summary>
    public readonly CancellationToken CancellationToken = cancellationToken;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<Diagnostic> Report = report;

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, Locatable location, params object?[]? messageArgs)
        => Report(Diagnostic.Create(descriptor, location.Location, messageArgs));
}
