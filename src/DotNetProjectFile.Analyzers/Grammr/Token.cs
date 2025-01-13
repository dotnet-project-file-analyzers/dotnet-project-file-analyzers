using Grammr.Text;
using Microsoft.CodeAnalysis.Text;

namespace Grammr;

/// <summary>Represents a single token.</summary>
public abstract class Token : Parser
{
    /// <summary>Initializes a new instance of the <see cref="Token"/> class.</summary>
    protected Token(string? kind) => Kind = kind ?? GetType().Name;

    /// <summary>The kind of the token.</summary>
    public string Kind { get; }

    /// <inheritdoc />
    public override Parser Option => new Optional(this);

    /// <summary>Indicates if a zero length token should be considered an optional or not.</summary>
    protected virtual bool AddZeroLengthToken => false;

    /// <inheritdoc />
    public override ResultQueue Parse(TokenStream stream, ResultQueue queue)
    {
        if (Match(stream.Remaining) is { } match)
        {
            if (match.Length > 0 || AddZeroLengthToken)
            {
                var add = stream.Add(match, Kind);
                var span = new SourceSpanToken(add[stream.Count].SourceSpan, Kind);
                var node = new Syntax.Token(span);
                return queue.Match(add, node);
            }
            else
            {
                return queue.Match(stream, null);
            }
        }
        else
        {
            return queue.NoMatch(stream, $"Expected {Kind}.");
        }
    }

    /// <summary>
    /// Returns the length of the matched token, and zero the source does not
    /// start with a matching token.
    /// </summary>
    /// <param name="source">
    /// The source text to match on.
    /// </param>
    [Pure]
    public abstract TextSpan? Match(SourceSpan source);

    private sealed class Optional(Token token) : Parser
    {
        public override ResultQueue Parse(TokenStream stream, ResultQueue queue)
        {
            if (token.Match(stream.Remaining) is { } match)
            {
                var add = stream.Add(match, token.Kind);
                var span = new SourceSpanToken(add[stream.Count].SourceSpan, token.Kind);
                var node = new Syntax.Token(span);
                return queue.Match(add, node);
            }
            else
            {
                return queue.Match(stream, null);
            }
        }
    }
}
