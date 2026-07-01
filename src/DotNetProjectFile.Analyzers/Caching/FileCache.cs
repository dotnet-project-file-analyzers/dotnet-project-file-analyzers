using System.Collections.Concurrent;

namespace DotNetProjectFile.Caching;

/// <summary>Caches items based on location.</summary>
/// <remarks>
/// Invalidation is based on the file's <see cref="IOFile.LastWriteTimeUtc"/>
/// combined with its length, so edits that keep the same write time
/// are still detected. Files without a readable stamp are never cached.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
public sealed class FileCache<T>() where T : class
{
    private readonly ConcurrentDictionary<IOFile, Entry> Lookup = new();

    /// <inheritdoc cref="ICollection.Count" />
    public int Count => Lookup.Count;

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

        public Stamp Version { get; set; }
    }

    /// <summary>Identifies a file version by write time and length.</summary>
    private readonly record struct Stamp(DateTime WriteTimeUtc, long Length)
    {
        /// <summary>Reads the stamp from a single <see cref="IOFile"/>, or null if unreadable.</summary>
        public static Stamp? For(IOFile path)
        {
            if (path.Info is { Exists: true } info)
            {
                try
                {
                    return new(info.LastWriteTimeUtc, info.Length);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
