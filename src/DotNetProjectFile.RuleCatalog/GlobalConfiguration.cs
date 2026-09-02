using System.IO;

namespace DotNetProjectFile.RuleCatalog;

/// <summary>Represents (global) analyzer configuration.</summary>
[DebuggerDisplay("Root = {Root}, Count = {Count}")]
public sealed class GlobalConfiguration : IReadOnlyCollection<GlobalConfigurationEntry>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private Dictionary<DiagnosticId, GlobalConfigurationEntry>.ValueCollection Entries => Lookup.Values;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Dictionary<DiagnosticId, GlobalConfigurationEntry> Lookup;

    /// <summary>Initializes a new instance of the <see cref="GlobalConfiguration"/> class.</summary>
    private GlobalConfiguration(Dictionary<DiagnosticId, GlobalConfigurationEntry> lookup) => Lookup = lookup;

    /// <inheritdoc />
    public int Count => Lookup.Count;

    /// <summary>Indicates if this should be the top level entry.</summary>
    public bool Root { get; init; } = true;

    /// <summary>Gets the entry if available.</summary>
    public GlobalConfigurationEntry? this[DiagnosticId id]
        => Lookup.TryGetValue(id, out var entry)
        ? entry
        : null;

    /// <summary>Indicates if the <see cref="DiagnosticId"/> is specified or not.</summary>
    [Pure]
    public bool Contains(DiagnosticId id) => Lookup.ContainsKey(id);

    /// <summary>Creates a new analyzer configuration applying the specified overrides.</summary>
    [Pure]
    public GlobalConfiguration Override(params GlobalConfigurationEntry[] overrides)
    {
        var updated = new GlobalConfiguration(new(Lookup)) { Root = Root };

        foreach (var o in overrides)
        {
            if (Lookup.TryGetValue(o.Id, out var existing))
            {
                if (o.Severity != existing.Severity || o.Justification is { Length: > 0 })
                {
                    updated.Lookup[o.Id] = existing with { Severity = o.Severity, Justification = o.Justification, IsOverride = true };
                }
            }
            else
            {
                updated.Lookup[o.Id] = o with { IsOverride = true };
            }
        }
        return updated;
    }

    /// <inheritdoc />
    [Pure]
    public IEnumerator<GlobalConfigurationEntry> GetEnumerator() => Entries
        .OrderBy(x => x)
        .GetEnumerator();

    /// <inheritdoc />
    [Pure]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc cref="Save(TextWriter, bool)" />
    public void Save(FileInfo file, bool includeDefaults = false)
    {
        ArgumentNullException.ThrowIfNull(file);

        using var writer = new StreamWriter(file.FullName, new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create });
        Save(writer, includeDefaults);
    }

    /// <inheritdoc cref="Save(TextWriter, bool)" />
    public void Save(Stream stream, bool includeDefaults = false)
    {
        ArgumentNullException.ThrowIfNull(stream);

        using var writer = new StreamWriter(stream);
        Save(writer, includeDefaults);
    }

    /// <summary>Save the analyzer configuration.</summary>
    public void Save(TextWriter writer, bool includeDefaults = false)
    {
        ArgumentNullException.ThrowIfNull(writer);

        writer.WriteLine("# Top level entry required to mark this as a global AnalyzerConfig file");
        writer.WriteLine("is_global = true");

        if (Root)
        {
            writer.WriteLine("root = true");
        }
        else
        {
            writer.WriteLine("root = false");
        }
        writer.WriteLine();

        writer.WriteLine("# .NET diagnostics overrides");
        GlobalConfigurationEntry? prev = null;

        foreach (var d in this.TakeWhile(d => d.IsOverride || includeDefaults))
        {
            if (prev is { })
            {
                if (prev.IsOverride != d.IsOverride)
                {
                    writer.WriteLine();
                    writer.WriteLine("# .NET diagnostics defaults");
                }
                else if (prev.Id.Prefix != d.Id.Prefix || prev.Severity != d.Severity)
                {
                    writer.WriteLine();
                }
            }
            writer.WriteLine(d);
            prev = d;
        }
    }

    /// <summary>Creates new analyzer configuration based on <see cref="DiagnosticInfo"/>.</summary>
    [Pure]
    public static GlobalConfiguration New(bool root, IEnumerable<DiagnosticInfo> diagnostics)
    {
        var ecd = new GlobalConfiguration([]) { Root = root };

        foreach (var diagnostic in diagnostics)
        {
            ecd.Lookup[diagnostic.Id] = GlobalConfigurationEntry.From(diagnostic);
        }
        return ecd;
    }
}
