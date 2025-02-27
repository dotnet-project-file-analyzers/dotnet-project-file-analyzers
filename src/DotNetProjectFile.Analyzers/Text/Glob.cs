using DotNetProjectFile.Text.Globbing;
using System.ComponentModel;

namespace DotNetProjectFile.Text;

[TypeConverter(typeof(Conversion.GlobTypeConverter))]
public readonly struct Glob
{
    private readonly Segment Segment;

    private Glob(Segment segment) => Segment = segment;

    public override string ToString() => Segment?.ToString() ?? string.Empty;

    /// <summary>Returns true if the segment matches the value.</summary>
    [Pure]
    public bool IsMatch(string value) => IsMatch(value, StringComparison.Ordinal);

    /// <inheritdoc cref="IsMatch(string)" />
    [Pure]
    public bool IsMatch(string value, StringComparison comparison) => Segment.IsMatch(value.AsSpan(), comparison);

    public static Glob? TryParse(string? expression)
        => expression is { Length: > 0 }
        && GlobParser.TryParse(expression) is { } segment
            ? new Glob(segment)
            : null;

    
}
