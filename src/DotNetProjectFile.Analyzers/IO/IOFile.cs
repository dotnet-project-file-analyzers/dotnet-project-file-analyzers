#pragma warning disable CA2231 // Overload operator equals on overriding value type Equals
#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"

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
#pragma warning disable S2365 // Properties should not make collection or array copies
        => HasValue
        ? new(Parts.Take(Parts.Length - 1).ToArray())
        : throw new InvalidOperationException("Path is empty");
#pragma warning restore S2365 // Properties should not make collection or array copies

    /// <summary>Creates a <see cref="FileInfo"/> based on the path.</summary>
    public FileInfo? Info
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
    public bool Exists => Info?.Exists ?? false;

    /// <inheritdoc cref="FileSystemInfo.LastWriteTime" />
    public DateTime? LastWriteTime => Get<DateTime?>(static info => info.LastWriteTime);

    /// <inheritdoc cref="FileSystemInfo.LastWriteTimeUtc" />
    public DateTime? LastWriteTimeUtc => Get<DateTime?>(static info => info.LastWriteTimeUtc);

    /// <inheritdoc cref="FileSystemInfo.LastAccessTime" />
    public DateTime? LastAccessTime => Get<DateTime?>(static info => info.LastAccessTime);

    /// <inheritdoc cref="FileSystemInfo.LastAccessTimeUtc" />
    public DateTime? LastAccessTimeUtc => Get<DateTime?>(static info => info.LastAccessTimeUtc);

    private T? Get<T>(Func<FileInfo, T> getter)
    {
        if (Info is null)
        {
            return default;
        }

        try
        {
            return getter(Info);
        }
        catch
        {
            return default;
        }
    }

    /// <inheritdoc cref="FileInfo.OpenText()" />
    public TextReader OpenText() => Info?.OpenText() ?? throw new FileNotFoundException();

    /// <summary>Tries <see cref="OpenText()"/>.</summary>
    /// <remarks>
    /// Returns a reader over <see cref="Stream.Null"/> if reading fails.
    /// </remarks>
    public TextReader TryOpenText()
    {
        try
        {
            return OpenText();
        }
        catch
        {
            return new StreamReader(Stream.Null);
        }
    }

    /// <summary>
    /// Reads all characters from the file.
    /// </summary>
    /// <returns>The file content.</returns>
    public string ReadAllText()
    {
        using var reader = OpenText();
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Reads all characters from the file.
    /// </summary>
    /// <returns>The file content.</returns>
    public string TryReadAllText()
    {
        using var reader = TryOpenText();
        return reader.ReadToEnd();
    }

    /// <inheritdoc cref="FileInfo.OpenRead()" />
    public FileStream OpenRead() => Info?.OpenRead() ?? throw new FileNotFoundException();

    /// <summary>Tries <see cref="OpenRead()"/>.</summary>
    /// <remarks>
    /// Returns <see cref="Stream.Null"/> if reading fails.
    /// </remarks>
    public Stream TryOpenRead()
    {
        try
        {
            return OpenRead();
        }
        catch
        {
            return Stream.Null;
        }
    }

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
