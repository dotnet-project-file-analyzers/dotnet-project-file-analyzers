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
    public T? TryGetOrUpdate(IOFile path, Func<IOFile, T?> create) => path switch
    {
        _ when !path.HasValue || !path.Exists => Remove(path),
        _ when Lookup.TryGetValue(path, out var entry) && entry.Version == path.LastWriteTimeUtc => entry.Value,
        _ when create(path) is { } file => Update(path, file),
        _ => Remove(path),
    };

    private T Update(IOFile path, T file)
    {
        Lookup[path] = new(path.LastWriteTimeUtc, file);
        return file;
    }

    /// <summary>Remove, as we can not longer resolve it.</summary>
    private T? Remove(IOFile path)
    {
        Lookup.TryRemove(path, out _);
        return default;
    }

    private readonly record struct Entry(DateTime? Version, T Value);
}
