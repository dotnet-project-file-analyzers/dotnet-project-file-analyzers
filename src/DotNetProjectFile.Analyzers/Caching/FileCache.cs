using System.Collections.Concurrent;

namespace DotNetProjectFile.Caching;

/// <summary>Caches items based on location.</summary>
/// <remarks>
/// The <see cref="IOFile.LastWriteTimeUtc"/> is used for invalidation.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
public sealed class FileCache<T>() where T : class
{
    private readonly ConcurrentDictionary<IOFile, Entry> Lookup = new();

    /// <inheritdoc cref="ICollection.Count" />
    public int Count => Lookup.Count;

    /// <summary>Tries to get the current file content for a specified file.</summary>
    /// <remarks>
    /// Uses the cased version if possible, otherwise creates/updates the file.
    /// </remarks>
    public T? TryGetOrUpdate(IOFile file, Func<IOFile, T> create)
    {
        if (!file.HasValue || !file.Exists)
        {
            return default;
        }
        else if (Lookup.TryGetValue(file, out var entry) && entry.Version == file.LastWriteTimeUtc)
        {
            return entry.Value;
        }
        else
        {
            entry = new(file.LastWriteTimeUtc, create(file));
            Lookup[file] = entry;
            return entry.Value;
        }
    }

    private readonly record struct Entry(DateTime Version, T Value);
}
