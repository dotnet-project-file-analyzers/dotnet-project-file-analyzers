using DotNetProjectFile.Collections;

namespace DotNetProjectFile.Text.Globbing;

internal sealed class Group(IReadOnlyList<Segment> segments) : Segment
{
    public IReadOnlyList<Segment> Segments { get; } = segments;

    /// <inheritdoc />
    public override int MinLength => Segments.Sum(s => s.MinLength);

    /// <inheritdoc />
    public override int MaxLength => Max(Segments);

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison)
        => IsMatch(Segments.AsSlice(), value, comparison);

    [Pure]
    private static bool IsMatch(Slice<Segment> segments, ReadOnlySpan<char> value, StringComparison comparison)
    {
        var (minLength, maxLength) = Lengths(segments);
        var buffer = value;

        if (value.Length < minLength || value.Length > maxLength) return false;

        while (segments.Count > 1)
        {
            var segment = segments[0];

            if (segment is { HasFixedLength: true })
            {
                if (!segment.IsMatch(buffer[..segment.MinLength], comparison)) return false;
                segments = segments.Skip(1);
                buffer = buffer[segment.MinLength..];
            }

            // If the last is fixed, we do that one first.
            else if (segments[^1] is { HasFixedLength: true } last)
            {
                if (!last.IsMatch(buffer[^last.MinLength..], comparison)) return false;
                segments = segments.Take(segments.Count - 1);
                buffer = buffer[..^last.MinLength];
            }
            else
            {
                segments = segments.Skip(1);
                (minLength, maxLength) = Lengths(segments);
                var min = Math.Max(0, buffer.Length - maxLength);
                var max = buffer.Length - minLength;

                for (var len = min; len <= max; len++)
                {
                    if (segment.IsMatch(buffer[..len], comparison) &&
                        IsMatch(segments, buffer[len..], comparison))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        return segments[0].IsMatch(buffer, comparison);
    }

    /// <inheritdoc />
    public override string ToString() => string.Concat(Segments);

    private static (int Min, int Max) Lengths(IEnumerable<Segment> segments)
        => (segments.Sum(s => s.MinLength), Max(segments));

    private static int Max(IEnumerable<Segment> segments)
    {
        var sum = 0;
        foreach (var maxLength in segments.Select(s => s.MaxLength))
        {
            if (maxLength == int.MaxValue) return int.MaxValue;
            sum += maxLength;
        }
        return sum;
    }
}
