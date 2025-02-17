#pragma warning disable CA2231 // Overload operator equals on overriding value type Equals
#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"

using System.ComponentModel;
using System.IO;

namespace DotNetProjectFile.IO;

/// <summary>Represents an (IO) directory.</summary>
[TypeConverter(typeof(Conversion.IODirectoryTypeConverter))]
public readonly struct IODirectory : IEquatable<IODirectory>, IFormattable, IComparable<IODirectory>
{
    /// <summary>Represents none/an empty path.</summary>
    public static readonly IODirectory Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string[]? _parts;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string[] Parts => _parts ?? [];

    internal IODirectory(string[] paths) => _parts = paths;

    public bool HasValue => Parts.Length != 0;

    /// <summary>Creates a <see cref="DirectoryInfo"/> based on the path.</summary>
    public DirectoryInfo? Info
    {
        get
        {
            try
            {
                return new(ToString());
            }
            catch
            {
                return null;
            }
        }
    }

    /// <inheritdoc cref="DirectoryInfo.Exists" />
    public bool Exists => Info?.Exists ?? false;

    /// <inheritdoc cref="FileSystemInfo.LastWriteTime" />
    public DateTime LastWriteTime => Info?.LastWriteTime ?? throw new DirectoryNotFoundException();

    /// <inheritdoc cref="FileSystemInfo.LastWriteTimeUtc" />
    public DateTime LastWriteTimeUtc => Info?.LastWriteTimeUtc ?? throw new DirectoryNotFoundException();

    /// <inheritdoc cref="FileSystemInfo.LastAccessTime" />
    public DateTime LastAccessTime => Info?.LastAccessTime ?? throw new DirectoryNotFoundException();

    /// <inheritdoc cref="FileSystemInfo.LastAccessTimeUtc" />
    public DateTime LastAccessTimeUtc => Info?.LastAccessTimeUtc ?? throw new DirectoryNotFoundException();

    public IODirectory Parent
#pragma warning disable S2365 // Properties should not make collection or array copies
        => HasValue
        ? new(Parts.Take(Parts.Length - 1).ToArray())
        : throw new InvalidOperationException("Path is empty");
#pragma warning restore S2365 // Properties should not make collection or array copies

    /// <summary>Creates a new sub directory.</summary>
    public IODirectory SubDirectory(params string[] paths)
        => Parts.Length == 0
            ? new(IOPath.Split(paths))
            : new(IOPath.Split(_parts.Concat(paths)));

    public IEnumerable<IODirectory>? SubDirectories(string path = ".")
    {
        try
        {
            return IterateDirectories(path)?.Select(f => IODirectory.Parse(f.FullName));
        }
        catch
        {
            return null;
        }
    }

    /// <summary>Creates a new file.</summary>
    public IOFile File(params string[] paths)
        => Parts.Length == 0
            ? new(IOPath.Split(paths))
            : new(IOPath.Split(_parts.Concat(paths)));

    public IEnumerable<IOFile>? Files(string path = ".")
    {
        try
        {
            return IterateFiles(path)?.Select(f => IOFile.Parse(f.FullName));
        }
        catch
        {
            return null;
        }
    }

    public IEnumerable<IODirectory> AncestorsAndSelf()
    {
        var current = this;

        while (current.HasValue)
        {
            yield return current;
            current = current.Parent;
        }
    }

    /// <summary>Gets the name of the directory.</summary>
    public string Name
        => Parts.Length == 0
        ? string.Empty
        : Parts[^1];

    /// <inheritdoc />
    public int CompareTo(IODirectory other) => string.Compare(ToString(), other.ToString(), true);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is IODirectory other && Equals(other);

    /// <inheritdoc />
    public bool Equals(IODirectory other) => Equals(other, IOPath.IsCaseSensitive);

    public bool Equals(IODirectory other, bool caseSensitive)
        => IOPath.Equals(Parts, other.Parts, caseSensitive);

    /// <inheritdoc />
    public override int GetHashCode() => IOPath.GetHashCode(Parts);

    /// <inheritdoc />
    public override string ToString() => ToString(null, null);

    public string ToString(string? format) => ToString(format, null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider) => IOPath.ToString(Parts, format, formatProvider);

    public static IODirectory Parse(string? str)
        => str?.Trim() is { Length: > 0 } trimmed
        ? new(IOPath.Split([trimmed]))
        : Empty;

    [Pure]
    private IEnumerable<FileInfo>? IterateFiles(string path)
        => Iterate(path, static (d, l) => d.EnumerateFiles(l));


    [Pure]
    private IEnumerable<DirectoryInfo>? IterateDirectories(string path)
        => Iterate(path, static (d, l) => d.EnumerateDirectories(l));

    [Pure]
    private IEnumerable<T>? Iterate<T>(string path, Func<DirectoryInfo, string, IEnumerable<T>> enumerate)
    {
        // We do not support variables yet.
        if (path.Contains("$(") || Info is null) return null;

        IEnumerable<DirectoryInfo> enumerator = new RootDirectory(Info);
        var parts = path.Split('/', '\\');

        foreach (var part in parts.Take(parts.Length - 1))
        {
            if (part == ".")
            {
                // keep current.
            }
            else if (part == "..")
            {
                if (enumerator is RootDirectory root)
                {
                    enumerator = root.Parent;
                }
                else
                {
                    enumerator = enumerator.Select(d => d.Parent!);
                }
            }
            else if (part == "**")
            {
                enumerator = enumerator.Concat(enumerator.SelectMany(d => d.EnumerateDirectories("*", SearchOption.AllDirectories)));
            }
            else
            {
                enumerator = enumerator.Concat(enumerator.SelectMany(d => d.EnumerateDirectories(part, SearchOption.TopDirectoryOnly)));
            }
        }

        var last = parts[^1];

        return enumerator
            .SelectMany(d => enumerate(d, last));
    }

    private sealed class RootDirectory(DirectoryInfo root) : IEnumerable<DirectoryInfo>
    {
        private readonly DirectoryInfo Root = root;

        public RootDirectory Parent => new(Root.Parent);

        [Pure]
        public IEnumerator<DirectoryInfo> GetEnumerator() { yield return Root; }

        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
