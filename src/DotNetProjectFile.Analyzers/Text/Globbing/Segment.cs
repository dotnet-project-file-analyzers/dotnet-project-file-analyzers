namespace DotNetProjectFile.Text.Globbing;

internal abstract class Segment
{
    public static readonly Segment AnyChar = new AnyChar();
    public static readonly Segment RecursiveWildcard = new RecursiveWildcard();
    public static readonly Segment Wildcard = new Wildcard();

    public static Segment Group(IReadOnlyList<Segment> segments) => new Group(segments);

    /// <summary>The minimum length the segment will match.</summary>
    public abstract int MinLength { get; }

    /// <summary>The maximum length the segment will match.</summary>
    public abstract int MaxLength { get; }

    /// <inheritdoc />
    public bool HasFixedLength => MinLength == MaxLength;

    /// <inheritdoc cref="Glob.IsMatch(string, StringComparison)" />
    [Pure]
    public abstract bool IsMatch(ReadOnlySpan<char> value, StringComparison comparison);
}
