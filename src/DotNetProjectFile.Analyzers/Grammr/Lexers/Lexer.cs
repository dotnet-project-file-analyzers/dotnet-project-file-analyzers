namespace Grammr.Lexers;

public abstract class Lexer(string? kind)
{
    /// <summary>Lexer/token kind.</summary>
    public string? Kind { get; } = kind;

    /// <summary>Tries to match, and returns the length of that match when succesful.</summary>
    [Pure]
    public abstract int? Match(SourceReader reader);

    /// <summary>Makes the lexer optional, meaning it will match zero or one occurrence of the pattern.</summary>
    public virtual Lexer optional => field ??= new Optional(this, Kind);

    public static Lexer operator |(Lexer l, Lexer r) => new Choice([l, r]);

    [Pure]
    protected static string Format(object obj) => Formatter.Format(obj);
}
