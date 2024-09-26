#pragma warning disable CA2231 // Overload operator equals on overriding value type Equals
#pragma warning disable S1210 // "Equals" and the comparison operators should be overridden when implementing "IComparable"

using System.IO;

namespace DotNetProjectFile.IO;

/// <summary>Represents an (IO) path.</summary>
public static class IOPath
{
    /// <inheritdoc cref="Path.DirectorySeparatorChar" />
    public static char Separator => Path.DirectorySeparatorChar;

    /// <summary>Returns true if the file system is case sensitive.</summary>
    public static readonly bool IsCaseSensitive = InitCaseSensitivity();

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

    internal static string ToString(string[] parts, string? format, IFormatProvider? formatProvider) => format switch
    {
        "/" => string.Join("/", parts),
        "\\" => string.Join("\\", parts),
        null => string.Join(Separator.ToString(), parts),
        _ => throw new FormatException($"The format '{format}' is a not supported directory separator char."),
    };

    internal static string[] Split(IEnumerable<string> parts)
    {
        var splitted = new List<string>();

        var many = parts.SelectMany(p => p.Split(Separators)).ToArray();

        var current = many.FirstOrDefault() == ".";

        foreach (var part in many.Skip(current ? 1 : 0))
        {
            if (part == ".")
            {
                // ignore.
            }
            // resolve .. if possible.
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

    private static readonly char[] Separators = ['/', '\\'];

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
