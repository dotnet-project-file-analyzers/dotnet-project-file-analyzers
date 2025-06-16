using Warpstone;
using static Warpstone.Parsers;

namespace DotNetProjectFile.Ini;

/// <summary>
/// Provides parsing logic for ini files.
/// </summary>
public static class IniParser
{
    private static readonly IParser<IniSyntaxWhitespace> Whitespace
        = Regex(@"[ \t]*")
        .AsResult()
        .Transform(static result => new IniSyntaxWhitespace
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,
            Value = result.Value,
        });

    private static readonly IParser<IniSyntaxKeyValuePairSeperator> Seperator
        = Or(Char('='), Char(':'))
        .AsResult()
        .Transform(static result => new IniSyntaxKeyValuePairSeperator
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,
            Character = result.Value,
        });

    private static readonly IParser<IniSyntaxComment> Comment
        = Or(Char(';'), Char('#'))
        .ThenAdd(Regex(@"[^\n\r]*"))
        .AsResult()
        .Transform(static result => new IniSyntaxComment
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,
            Indicator = result.Value.Left,
            Text = result.Value.Right,
        });

    private static readonly IParser<IniSyntaxLineComment> LineComment
        = Comment
        .Transform(static result => new IniSyntaxLineComment
        {
            StartPosition = result.StartPosition,
            EndPosition = result.EndPosition,
            Comment = result,
        });

    private static readonly IParser<IniSyntaxSectionHeader> LineSectionHeader
        = Char('[')
        .Then(Regex(@".*(?=])").AsResult())
        .ThenSkip(Char(']'))
        .Transform(static result => new IniSyntaxSectionHeader
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,
            Text = result.Value,
        });

    private static readonly IParser<IniSyntaxExpression> Expression
        = Regex(@"([^\s=;#]|(?<=[^\s=;#])([ \t]*(?=[^\s=;#])))*")
        .AsResult()
        .Transform(static result => new IniSyntaxExpression
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,
            Text = result.Value,
        });

    private static readonly IParser<IniSyntaxKeyValuePair> LineKeyValuePair
        = Expression
        .ThenAdd(Whitespace)
        .ThenAdd(Seperator)
        .ThenAdd(Whitespace)
        .ThenAdd(Expression)
        .AsResult()
        .Transform(static result => new IniSyntaxKeyValuePair
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,
            Key = result.Value.First,
            PostKeyWhitespace = result.Value.Second,
            Seperator = result.Value.Third,
            PreValueWhitespace = result.Value.Fourth,
            Value = result.Value.Fifth,
        });

    private static readonly IParser<IniSyntaxLineEmpty> LineEmpty
        = Whitespace
        .AsResult()
        .Transform(static result => new IniSyntaxLineEmpty
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,
        });

    private static readonly IParser<IniSyntaxLineContent> LineContent
        = Or<IniSyntaxLineContent>(LineSectionHeader, LineKeyValuePair, LineComment, LineEmpty);

    private static readonly IParser<IniSyntaxLine> Line
        = Whitespace
        .ThenAdd(LineContent)
        .ThenAdd(Whitespace)
        .ThenAdd(Maybe(Comment))
        .AsResult()
        .Transform(static result => new IniSyntaxLine
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,

            LeadingWhitespace = result.Value.First,
            Content = result.Value.Second,
            TrailingWhitespace = result.Value.Third,
            Comment = result.Value.Fourth,
        });

    private static readonly IParser<IImmutableList<IniSyntaxLine>> Lines
        = Many(Line);

    private static readonly IParser<IniSyntaxFile> File
        = Lines
        .ThenEnd()
        .AsResult()
        .Transform(static result => new IniSyntaxFile
        {
            StartPosition = result.InputStartPosition,
            EndPosition = result.InputEndPosition,

            Lines = result.Value,
        });

    public static IniSyntaxFile Parse(string text)
        => File.Parse(text);
}

public abstract record IniSyntaxNode
{
    public required ParseInputPosition StartPosition { get; init; }

    public required ParseInputPosition EndPosition { get; init; }
}

public sealed record IniSyntaxWhitespace : IniSyntaxNode
{
    public required string Value { get; init; }
}

public sealed record IniSyntaxLine : IniSyntaxNode
{
    public required IniSyntaxWhitespace LeadingWhitespace { get; init; }

    public required IniSyntaxLineContent Content { get; init; }

    public required IniSyntaxWhitespace TrailingWhitespace { get; init; }

    public required IniSyntaxComment? Comment { get; init; }
}

public sealed record IniSyntaxComment : IniSyntaxNode
{
    public required char Indicator { get; init; }

    public required string Text { get; init; }
}

public abstract record IniSyntaxLineContent : IniSyntaxNode
{

}

public sealed record IniSyntaxLineComment : IniSyntaxLineContent
{
    public required IniSyntaxComment Comment { get; init; }
}

public sealed record IniSyntaxLineEmpty : IniSyntaxLineContent
{
}

public sealed record IniSyntaxSectionHeader : IniSyntaxLineContent
{
    public required string Text { get; init; }
}

public sealed record IniSyntaxKeyValuePair : IniSyntaxLineContent
{
    public required IniSyntaxExpression Key { get; init; }

    public required IniSyntaxWhitespace PostKeyWhitespace { get; init; }

    public required IniSyntaxKeyValuePairSeperator Seperator { get; init; }

    public required IniSyntaxWhitespace PreValueWhitespace { get; init; }

    public required IniSyntaxExpression Value { get; init; }
}

public sealed record IniSyntaxExpression : IniSyntaxNode
{
    public required string Text { get; init; }
}

public sealed record IniSyntaxKeyValuePairSeperator : IniSyntaxNode
{
    public required char Character { get; init; }
}

public sealed record IniSyntaxFile : IniSyntaxNode
{
    public IImmutableList<IniSyntaxLine> Lines { get; init; }
}
