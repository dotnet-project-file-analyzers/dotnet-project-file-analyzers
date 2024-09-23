namespace DotNetProjectFile.CodeAnalysis;

/// <summary>Represents a fixed set of #pragma warning's for a MS Build project file.</summary>
[DebuggerTypeProxy(typeof(CollectionDebugView))]
[DebuggerDisplay("Count = {Count}")]
public readonly struct WarningPragmas(IReadOnlyCollection<WarningPragma> warnings) : IReadOnlyCollection<WarningPragma>
{
    public static readonly WarningPragmas None = new([]);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IReadOnlyCollection<WarningPragma> Warnings = warnings;

    /// <inheritdoc />
    public int Count => Warnings.Count;

    /// <summary>Returns true if the requested diagnostic is disabled at the given location.</summary>
    public bool IsDisabled(string diagnosticId, Location location) => Warnings.FirstOrDefault(w
        => w.DiagnosticId == diagnosticId
        && w.Location.SourceSpan.Start < location.SourceSpan.Start).IsDisabled;

    public static WarningPragmas New(MsBuildProject project)
    => new([.. project.Element
            .DescendantNodes()
            .OfType<XComment>()
            .Select(c => WarningPragma.New(c, project))
            .OfType<WarningPragma>()
            .OrderByDescending(p => p.Location.SourceSpan.Start)]);

    /// <inheritdoc />
    public IEnumerator<WarningPragma> GetEnumerator() => Warnings.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
