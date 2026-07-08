using AwesomeAssertions.Execution;
using System.Diagnostics;
using Token = Grammr.Token;

namespace Specs.TestTools;

internal sealed class TokenAssertions(Token? subject, AssertionChain? chain = null)
{
    public Token? Subject { get; } = subject;

    private readonly AssertionChain Assert = chain ?? AssertionChain.GetOrCreate();

    [DebuggerStepThrough]
    public AndConstraint<TokenAssertions> Match(TokenInfo match, string because = "")
    {
        var token = Subject.GetValueOrDefault();
        Assert
            .ForCondition(Subject is not null)
            .BecauseOf(because)
            .FailWith("Expected {context:token} to match{reason}, but it did not.")
            .Then
            .ForCondition(match.Span == token.Span.ToString() && match.Kind == token.Kind)
            .FailWith(
                "Expected {context:token} to match {0}{reason}, but it matched {1} instead.",
                Formatter.Format(match.Span, match.Kind),
                Formatter.Format(token.Span.ToString(), token.Kind));

        return new(this);
    }

    [DebuggerStepThrough]
    public AndConstraint<TokenAssertions> NotMatch(string because = "")
    {
        Assert
            .ForCondition(Subject is null)
            .BecauseOf(because)
            .FailWith(
                "Expected {context:lexer} not to match{reason}, but it did match {0}.",
                Formatter.Format(Subject?.Span.ToString()!, Subject?.Kind));

        return new(this);
    }
}

internal readonly record struct TokenInfo(string Span, string? Kind = null);
