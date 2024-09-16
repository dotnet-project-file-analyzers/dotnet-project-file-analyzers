using DotNetProjectFile.Parsing;
using Microsoft.CodeAnalysis.Text;

namespace FluentAssertions;

internal sealed class ParserAssertions(Parser subject)
{
    public Parser Subject { get; } = subject;

    public void HaveTokenized(params Token[]? expected)
    {
        Subject.State.Should().Be(Matching.EoF, because: "Did not fully tokenized.");
        if (expected is { })
        {
            var tokens = Subject.Tokens.ToArray();
            tokens.Should().BeEquivalentTo(expected);
        }
    }

    public void NotHaveTokenized(params Token[] expected)
    {
        Subject.State.Should().Be(Matching.NoMatch, because: "Did not fully tokenized.");
        var tokens = Subject.Tokens.ToArray();
        tokens.Should().BeEquivalentTo(expected);
    }
}

internal readonly record struct Token(TextSpan Span, string Text, string? Kind)
{
    public static Token New(int start, string text, string? kind = null)
        => new(new(start, text.Length), text, kind);
}
