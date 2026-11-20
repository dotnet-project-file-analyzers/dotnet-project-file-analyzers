using Microsoft.CodeAnalysis.Text;
using System.Collections.Concurrent;

namespace DotNetProjectFile.Caching;

/// <summary>Caches items based on location.</summary>
/// <remarks>
/// For <see cref="AdditionalText"/>s invalidation is based on their <see cref="SourceText"/>,
/// so unsaved IDE edits are picked up. Otherwise invalidation is based on the file's
/// <see cref="IOFile.LastWriteTimeUtc"/> combined with its length, so edits that keep
/// the same write time are still detected. Files without a readable stamp are never cached.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
public sealed class FileCache<T>() where T : class
{
    private readonly ConcurrentDictionary<IOFile, Entry> Lookup = new();

    /// <inheritdoc cref="ICollection.Count" />
    public int Count => Lookup.Count;

    /// <inheritdoc cref="TryGetOrUpdate(AdditionalText, Func{IOFile, T?}, CancellationToken)" />
    public T? TryGetOrUpdate(AdditionalFileAnalysisContext context, Func<IOFile, T?> create)
        => TryGetOrUpdate(context.AdditionalFile, create, context.CancellationToken);

    /// <summary>Tries to get the current file content for a specified additional text.</summary>
    /// <remarks>
    /// The <see cref="SourceText"/> of an additional text is leading: in an IDE
    /// it reflects the (unsaved) editor buffer, which can differ from disk.
    /// Returns the cached value when it was created from the same text,
    /// otherwise creates/updates the entry. Removes the entry when the text
    /// is unavailable or when the transform fails.
    /// </remarks>
    public T? TryGetOrUpdate(AdditionalText additional, Func<IOFile, T?> create, CancellationToken cancellationToken = default)
    {
        var path = additional.Location;

        if (!path.HasValue || additional.GetText(cancellationToken) is not { } text)
        {
            return Remove(path);
        }

        var entry = Lookup.GetOrAdd(path, static _ => new());

        lock (entry)
        {
            // ponytail: reference equality; Roslyn reuses the SourceText instance while unchanged.
            // A false miss only costs a re-parse; compare checksums if misses show up in profiling.
            if (entry.Value is { } && ReferenceEquals(entry.Text, text))
            {
                return entry.Value;
            }
            else if (create(path) is { } file)
            {
                entry.Value = file;
                entry.Text = text;
                entry.Version = Stamp.For(path);
                return file;
            }
        }

        return Remove(path);
    }

    /// <summary>Tries to get the current file content for a specified file.</summary>
    /// <remarks>
    /// Returns the cached value when the file's last edit and length are unchanged,
    /// otherwise creates/updates the entry. Removes the entry when the file is gone,
    /// unreadable or when the transform fails.
    /// </remarks>
    public T? TryGetOrUpdate(IOFile path, Func<IOFile, T?> create)
    {
        if (!path.HasValue || Stamp.For(path) is not { } stamp)
        {
            return Remove(path);
        }

        var entry = Lookup.GetOrAdd(path, static _ => new());

        lock (entry)
        {
            if (entry.Value is { } && entry.Version == stamp)
            {
                return entry.Value;
            }
            else if (create(path) is { } file)
            {
                entry.Value = file;
                entry.Text = null;
                entry.Version = stamp;
                return file;
            }
        }

        return Remove(path);
    }

    /// <summary>Remove, as we can no longer resolve it.</summary>
    private T? Remove(IOFile path)
    {
        Lookup.TryRemove(path, out _);
        return default;
    }

    private sealed class Entry
    {
        public T? Value { get; set; }

        /// <summary>The additional text the value was created from, if any.</summary>
        public SourceText? Text { get; set; }

        /// <summary>The disk stamp when the value was created, if any.</summary>
        public Stamp? Version { get; set; }
    }

    /// <summary>Identifies a file version by write time and length.</summary>
    private readonly record struct Stamp(DateTime WriteTimeUtc, long Length)
    {
        /// <summary>Reads the stamp from a single <see cref="IOFile"/>, or null if unreadable.</summary>
        public static Stamp? For(IOFile path)
        {
            if (path.Info is not { } info)
            {
                return null;
            }

            try
            {
                return new(info.LastWriteTimeUtc, info.Length);
            }
            catch
            {
                return null;
            }
        }
    }
}
