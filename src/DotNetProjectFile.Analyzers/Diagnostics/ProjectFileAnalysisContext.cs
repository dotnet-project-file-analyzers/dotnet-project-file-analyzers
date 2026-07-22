using Microsoft.CodeAnalysis.Text;

namespace DotNetProjectFile.Diagnostics;

/// <summary>The context required to analyze a project file.</summary>
/// <typeparam name="TFile">
/// The type of file to analyze.
/// </typeparam>
public readonly struct ProjectFileAnalysisContext<TFile>(
    TFile file,
    AnalyzerType type,
    Compilation compilation,
    AnalyzerOptions options,
    CancellationToken cancellationToken,
    Action<Diagnostic> report)
    where TFile : ProjectFile
{
    [Obsolete("Set analyzertype")]
    public ProjectFileAnalysisContext(
        TFile file,
        Compilation compilation,
        AnalyzerOptions options,
        CancellationToken cancellationToken,
        Action<Diagnostic> report) : this(file, default, compilation, options, cancellationToken, report) { }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<Diagnostic> Report = report;

    /// <summary>Gets the project file.</summary>
    public TFile File { get; } = file;

    /// <summary>The type of the analyzer.</summary>
    public AnalyzerType AnalyzerType { get; } = type;

    /// <summary>Gets the compilation language.</summary>
    public Language CompilationLanguage => Language.Parse(Compilation.Language);

    /// <summary>Gets the compilation.</summary>
    public Compilation Compilation { get; } = compilation;

    /// <summary>Gets the analyzer options.</summary>
    public AnalyzerOptions Options { get; } = options;

    /// <summary>Gets the MS Build properties.</summary>
    public MsBuildProps Props { get; } = new(options);

    /// <summary>Gets the cancellation token.</summary>
    public CancellationToken CancellationToken { get; } = cancellationToken;

    /// <summary>Reports a diagnostic about the project file.</summary>
    public void ReportDiagnostic(DiagnosticDescriptor descriptor, XmlAnalysisNode node, params object?[]? messageArgs)
    {
#pragma warning disable S3215 // "interface" instances should not be cast to concrete types
        // Having a different overload should fix most issues, if not all, and currently
        // adding NoWarn to the interface is not worth it.
        if (NoWarn(node as Node, descriptor)) return;
#pragma warning restore S3215 // "interface" instances should not be cast to concrete types

        ReportDiagnostic(
           descriptor,
           node.Element.Parent is null ? node.Locations.StartElement : node.Locations.FullSpan,
           messageArgs);
    }

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

    private static bool NoWarn(Node? node, DiagnosticDescriptor descriptor)
        => node is not null
        && (node.NoWarn.Contains(descriptor.Id) || NoWarn(node.Parent, descriptor));
}
