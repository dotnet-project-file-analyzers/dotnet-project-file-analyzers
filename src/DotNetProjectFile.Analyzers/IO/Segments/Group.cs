namespace DotNetProjectFile.IO.Segments;

internal sealed class Group(Segment[] segments) : Segment
{
    public Segment[] Segments { get; } = segments;

    /// <inheritdoc />
    public override int MinLength => Segments.Sum(s => s.MinLength);

    /// <inheritdoc />
    public override int MaxLength => Max(Segments);

    /// <inheritdoc />
    [Pure]
    public override bool IsMatch(Chars value, StringComparison comparison)
        => IsMatch(Segments.AsSpan(), value, comparison);

    [Pure]
    private static bool IsMatch(ReadOnlySpan<Segment> segments, Chars value, StringComparison comparison)
    {
        var (minLength, maxLength) = Lengths(segments);
        var buffer = value;

        if (value.Length < minLength || value.Length > maxLength) return false;

        while (segments.Length > 1)
        {
            var segment = segments[0];

            if (segment is { HasFixedLength: true })
            {
                if (!segment.IsMatch(buffer[..segment.MinLength], comparison)) return false;
                segments = segments[1..];
                buffer = buffer[segment.MinLength..];
            }

            // If the last is fixed, we do that one first.
            else if (segments[^1] is { HasFixedLength: true } last)
            {
                if (!last.IsMatch(buffer[^last.MinLength..], comparison)) return false;
                segments = segments[..^1];
                buffer = buffer[..^last.MinLength];
            }
            else
            {
                segments = segments[1..];
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
    public override string ToString() => string.Concat(Segments.Select(s => s.ToString()));

    private static (int Min, int Max) Lengths(ReadOnlySpan<Segment> segments)
        => (Sum(segments), Max(segments));

    private static int Sum(ReadOnlySpan<Segment> segments)
    {
        var sum = 0;
        foreach (var segment in segments)
            sum += segment.MinLength;

        return sum;
    }

    private static int Max(ReadOnlySpan<Segment> segments)
    {
        var sum = 0;
        foreach (var segment in segments)
        {
            var maxLength = segment.MaxLength;
            if (maxLength == int.MaxValue) return int.MaxValue;
            sum += maxLength;
        }
        return sum;
    }
}
