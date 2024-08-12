#nullable enable

using System.ComponentModel;
using System.IO;

namespace DotNetProjectFile.IO;

/// <summary>Represents an (IO) file.</summary>
[TypeConverter(typeof(Conversion.IOFileTypeConverter))]
public readonly struct IOFile : IEquatable<IOFile>, IFormattable, IComparable<IOFile>
{
    /// <summary>Represents none/an empty path.</summary>
    public static readonly IOFile Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string[]? _parts;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string[] Parts => _parts ?? [];

    internal IOFile(string[] paths) => _parts = paths;

    public bool HasValue => Parts.Length != 0;

    /// <inheritdoc cref="FileInfo.Directory" />
    public IODirectory Directory
        => HasValue
        ? new(Parts.Take(Parts.Length - 1).ToArray())
        : throw new InvalidOperationException("Path is empty");

    /// <summary>Creates a <see cref="FileInfo"/> based on the path.</summary>
    private FileInfo File() => new(ToString());

    /// <summary>Creates a new path.</summary>
    public IOFile Combine(params string[] paths)
        => Parts.Length == 0
            ? new(IOPath.Split(paths))
            : new(IOPath.Split(_parts.Concat(paths)));

    /// <summary>Gets the extension of the path.</summary>
    public string Extension
        => Parts.Length == 0
        ? string.Empty
        : Path.GetExtension(Parts[^1]);

    /// <summary>Gets the name of the file.</summary>
    public string Name
        => Parts.Length == 0
        ? string.Empty
        : Parts[^1];

    /// <summary>Gets the name of the file without extension.</summary>
    public string NameWithoutExtension
        => Parts.Length == 0
        ? string.Empty
        : Path.GetFileNameWithoutExtension(Parts[^1]);

    /// <inheritdoc cref="FileInfo.Exists" />
    public bool Exists => File().Exists;

    /// <inheritdoc cref="FileInfo.OpenText()" />
    public TextReader OpenText() => File().OpenText();

    /// <inheritdoc />
    public int CompareTo(IOFile other) => string.Compare(ToString(), other.ToString(), true);

    /// <inheritdoc />
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is IOFile other && Equals(other);

    /// <inheritdoc />
    public bool Equals(IOFile other) => Equals(other, IOPath.IsCaseSensitive);

    public bool Equals(IOFile other, bool caseSensitive) => IOPath.Equals(Parts, other.Parts, caseSensitive);

    /// <inheritdoc />
    public override int GetHashCode() => IOPath.GetHashCode(Parts);

    /// <inheritdoc />
    public override string ToString() => ToString(null, null);

    public string ToString(string? format) => ToString(format, null);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider) 
        => IOPath.ToString(Parts, format, formatProvider);

    public static IOFile Parse(string? str)
        => str?.Trim() is { Length: > 0 } trimmed
        ? new(IOPath.Split([trimmed]))
        : Empty;
}
