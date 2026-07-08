using AwesomeAssertions.Execution;
using Grammr.Lexers;
using System.Diagnostics;

namespace Specs.TestTools;

internal sealed class LexerAssertions(Lexer subject, AssertionChain? chain = null)
{
    public Lexer Subject { get; } = subject;

    private readonly AssertionChain Assert = chain ?? AssertionChain.GetOrCreate();

    [DebuggerStepThrough]
    public AndConstraint<LexerAssertions> Match(string match, string from, string because = "")
    {
        var length = Subject.Match(new(from));

        Assert
            .ForCondition(length is { })
            .BecauseOf(because)
            .FailWith("Expected {context:lexer} to match {0}{reason}, but it did not.", from)
            .Then
            .ForCondition(match == from[..length.GetValueOrDefault()])
            .FailWith("Expected {context:lexer} to match {0}{reason}, but it matched {1} instead.", from, from[..length.GetValueOrDefault()]);

        return new(this);
    }

    [DebuggerStepThrough]
    public AndConstraint<LexerAssertions> NotMatch(string source, string because = "")
    {
        var length = Subject.Match(new(source));

        Assert
            .ForCondition(length is null)
            .BecauseOf(because)
            .FailWith("Expected {context:lexer} not to match {0}{reason}, but it did match {1}.", source, source[..length.GetValueOrDefault()]);

        return new(this);
    }
}
