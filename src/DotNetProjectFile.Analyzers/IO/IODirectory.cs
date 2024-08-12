#nullable enable

using System.ComponentModel;
using System.Diagnostics.Contracts;
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

    public IODirectory Parent
        => HasValue
        ? new(Parts.Take(Parts.Length - 1).ToArray())
        : throw new InvalidOperationException("Path is empty");

    /// <summary>Creates a new sub directory.</summary>
    public IODirectory SubDirectory(params string[] paths)
        => Parts.Length == 0
            ? new(IOPath.Split(paths))
            : new(IOPath.Split(_parts.Concat(paths)));

    /// <summary>Creates a new file.</summary>
    public IOFile File(params string[] paths)
        => Parts.Length == 0
            ? new(IOPath.Split(paths))
            : new(IOPath.Split(_parts.Concat(paths)));

    public IEnumerable<IOFile>? Files(string path)
    {
        try
        {
            return Iterate(new(ToString()), path)?.Select(f => IOFile.Parse(f.FullName));
        }
        catch
        {
            return null;
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
    private static IEnumerable<FileInfo>? Iterate(DirectoryInfo directory, string path)
    {
        // We do not support variables yet.
        if (path.Contains("$(")) return null;

        IEnumerable<DirectoryInfo> enumerator = new RootDirectory(directory);
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
                enumerator = enumerator.SelectMany(d => d.EnumerateDirectories("*", SearchOption.AllDirectories));
            }
            else
            {
                enumerator = enumerator.SelectMany(d => d.EnumerateDirectories(part, SearchOption.TopDirectoryOnly));
            }
        }
        return enumerator
            .SelectMany(d => d.EnumerateFiles(parts[^1]));
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
