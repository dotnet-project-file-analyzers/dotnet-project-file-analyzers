using System.IO;

namespace DotNetProjectFile.IO;

/// <summary>Represents an (IO) path.</summary>
public static class IOPath
{
    /// <inheritdoc cref="Path.DirectorySeparatorChar" />
    public static char Separator => Path.DirectorySeparatorChar;

    /// <summary>Returns true if the file system is case sensitive.</summary>
    public static readonly bool IsCaseSensitive = InitCaseSensitivity();

    private static readonly char[] Separators = ['/', '\\'];
    private static readonly char[] StartTrimCharacters = [' ', '\t', '\n', '\r'];
    private static readonly char[] EndTrimCharacters = [.. StartTrimCharacters, .. Separators];

    /// <summary>Checks if the selector and the file have the same casing.</summary>
    /// <returns>
    /// null if the same, else the part of file that matches the selector except for the casing.
    /// </returns>
    [Pure]
    public static string? CaseCompare(IOFile file, IOFile selector)
    {
        var same = true;
        var parts = new List<string>();

        var (fil, sel) = (file.ToArray(), selector.ToArray());
        var (f, s) = (fil.Length, sel.Length);

        while (f-- > 0 && s-- > 0)
        {
            if (sel[s] is "..") break;
            else if (sel[s] is not ".")
            {
                same &= sel[s] == fil[f];
                parts.Insert(0, fil[f]);
            }
        }

        return same ? null : string.Join("/", parts);
    }

    /// <summary>Returns true if the path is fully qualified.</summary>
    /// <remarks>
    /// System.IO.Path.IsPathFullyQualified(string) does not exist in .NET standard 2.0.
    /// </remarks>
    [Pure]
    public static bool IsFullyQualified(string? path)
        => path is { Length: > 0 }
        && Path.IsPathRooted(path)
        && (path[0] is '/' || path.StartsWith(@"\\") || (path.Length >= 2 && path[1] is ':'));

    internal static bool Equals(string[] self, string[] other, bool caseSensitive)
    {
        if (self.Length != other.Length) { return false; }

        for (var i = 0; i < self.Length; i++)
        {
            if (!string.Equals(self[i], other[i], caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
        }
        return true;
    }

    internal static int GetHashCode(string[] parts)
    {
        var code = 0;
        foreach (var part in parts)
        {
            code *= 1566083941;
            code += IsCaseSensitive ? part.GetHashCode() : part.ToUpperInvariant().GetHashCode();
        }
        return code;
    }

    internal static string ToString(string[] parts, string? format, IFormatProvider? _) => format switch
    {
        "/" => string.Join("/", parts),
        "\\" => string.Join("\\", parts),
        null => string.Join(Separator.ToString(), parts),
        _ => throw new FormatException($"The format '{format}' is a not supported directory separator char."),
    };

    internal static string[] Split(IEnumerable<string> parts)
    {
        var splitted = new List<string>();

        var many = parts
            .SelectMany(p => p
                .TrimStart(StartTrimCharacters)
                .TrimEnd(EndTrimCharacters)
                .Split(Separators))
            .ToArray();

        var current = many.FirstOrDefault() == ".";

        foreach (var part in many.Skip(current ? 1 : 0))
        {
            if (part == ".")
            {
                // ignore.
            }
            else if (part == ".." && splitted.Any() && splitted[^1] != "..")
            {
                splitted.RemoveAt(splitted.Count - 1);
            }
            else
            {
                splitted.Add(part);
            }
        }
        if (current)
        {
            splitted.Insert(0, ".");
        }
        return [.. splitted];
    }

    private static bool InitCaseSensitivity()
    {
        try
        {
            return !new FileInfo(typeof(IOPath).Assembly.Location.ToUpperInvariant()).Exists;
        }
        catch
        {
            return true;
        }
    }
}
