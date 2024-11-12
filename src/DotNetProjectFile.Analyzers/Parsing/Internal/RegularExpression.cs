using System.Text.RegularExpressions;

namespace DotNetProjectFile.Parsing.Internal;

internal sealed class RegularExpression(Regex pattern, string? kind, bool line)
    : Grammar
{
    internal RegularExpression(string pattern, string? kind, bool line)
        : this(Regex(pattern), kind, line) { }

    private readonly Regex Pattern = pattern;
    private readonly string? Kind = kind;
    private readonly bool Line = line;

    /// <inheritdoc />
    [Pure]
    public override Parser Match(Parser parser)
        => Line
        ? parser.Match(s => s.Line(Pattern), Kind)
        : parser.Match(s => s.Regex(Pattern), Kind);

    /// <inheritdoc />
    [Pure]
    public override string ToString() => string.IsNullOrEmpty(Kind) switch
    {
        true => Line ? $"line({Formatted})" : $"regex({Formatted})",
        _ => Line ? $"line('{Formatted}', {Kind})" : $"regex('{Formatted}', {Kind})",
    };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private string Formatted => Pattern.ToString().TrimStart('^');

    private static Regex Regex(string regex) => regex[0] == '^'
        ? new(regex, Options, Timeout)
        : new('^' + regex, Options, Timeout);

    private static readonly RegexOptions Options = RegexOptions.CultureInvariant;

    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds(10);
}
