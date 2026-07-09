namespace Grammr;

internal static class Formatter
{
    [Pure]
    public static string Format(object? obj)
        => obj?.ToString()?
        .Replace("\0", "\\0")
        .Replace("\n", "\\n")
        .Replace("\r", "\\r")
        .Replace("\t", "\\t")
        ?? string.Empty;
}
