using DotNetProjectFile.Collections;
using Grammr.Parsers;
using Grammr.Text;

namespace Grammr;

/// <summary>Represents a sequence of tokens.</summary>
public abstract class Parser
{
    public Syntax.Tree Parse(TokenStream stream)
    {
        var outcome = Parse(stream, new()).Outcome;
        return new((outcome.Node as Syntax.Node)!, outcome.Stream, outcome.Message);
    }

    /// <summary>Tokenizes the source span.</summary>
    public abstract ResultQueue Parse(TokenStream stream, ResultQueue queue);

    /// <summary>Creates a switch of tokens to choose one of.</summary>
    public static Parser operator |(Parser l, Parser r) => new Parsers.Switch([l, r]);

    /// <summary>Creates a required sequence of tokens.</summary>
    public static Parser operator &(Parser l, Parser r) => new Sequence([l, r]);

#if NETSTANDARD2_0
    internal Parser this[Range range] => new Repeat(this, range.Start.Value, range.End.Value);

    public Parser Repeat(int min, int max = int.MaxValue) => new Repeat(this, min, max);
#else
    public Tokens this[Range range] => new Repeat(this, range.Start.Value, range.End.Value);
#endif

    /// <summary>The grammar may or may not match.</summary>
    public virtual Parser Option => new Repeat(this, 0, 1);

    /// <summary>This grammar may match multiple times.</summary>
    public Repeat Star => new(this, 0, int.MaxValue);

    /// <summary>This grammar may match multiple times, but at least once.</summary>
    public Repeat Plus => new(this, 1, int.MaxValue);

    protected static Syntax.Node? Select(AppendOnlyList<Syntax.Node> nodes) => nodes.Count switch
    {
        0 => null,
        1 => nodes[0],
        _ => Syntax.Node.New(nodes),
    };
}
