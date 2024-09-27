using static DotNetProjectFile.IO.Globbing.Segement;

namespace DotNetProjectFile.IO.Globbing;

public abstract class Segement
{
    public static readonly Segement AnyChar = new AnyChar();
    public static readonly Segement RecursiveWildcard = new RecursiveWildcard();
    public static readonly Segement Wildcard = new Wildcard();

    public static Segement Group(IReadOnlyList<Segement> segments) => new Group(segments);

    /// <summary>The minimum length the segment will match.</summary>
    public abstract int MinLength { get; }

    /// <summary>The maximum length the segment will match.</summary>
    public abstract int MaxLength { get; }
}
