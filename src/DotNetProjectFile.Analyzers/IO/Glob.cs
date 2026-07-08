using DotNetProjectFile.IO.Segments;
using Grammr;
using System.ComponentModel;

namespace DotNetProjectFile.IO;

[TypeConverter(typeof(Conversion.GlobTypeConverter))]
public readonly struct Glob
{
    private readonly Segment Segment;

    internal Glob(Segment segment) => Segment = segment;

    public override string ToString() => Segment?.ToString() ?? string.Empty;

    /// <summary>Returns true if the segment matches the value.</summary>
    [Pure]
    public bool IsMatch(string value) => IsMatch(value, StringComparison.Ordinal);

    /// <inheritdoc cref="IsMatch(string)" />
    [Pure]
    public bool IsMatch(string value, StringComparison comparison) => Segment.IsMatch(value.AsSpan(), comparison);

    public static Glob? TryParse(string? expression)
    {
        if (expression is not { Length: > 0 }) return default;

        var reader = new SourceReader(expression);

        return GlobParser.Parse(ref reader) is { } segment && reader.EOS
            ? new Glob(segment)
            : null;
    }
}
