using DotNetProjectFile.Text.Globbing;
using System.ComponentModel;

namespace DotNetProjectFile.Text;

[TypeConverter(typeof(Conversion.GlobTypeConverter))]
public readonly struct Glob
{
    private readonly Segement Segment;

    private Glob(Segement segment) => Segment = segment;

    public override string ToString() => Segment?.ToString() ?? string.Empty;

    public static Glob? TryParse(string? expression)
        => expression is { Length: > 0 }
        && GlobParser.TryParse(expression) is { } segment
            ? new Glob(segment)
            : null;
}
