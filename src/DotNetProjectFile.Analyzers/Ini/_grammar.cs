using DotNetProjectFile.Parsing;
using static DotNetProjectFile.Ini.TokenKind;

namespace DotNetProjectFile.Ini;

internal sealed class IniGrammar : Grammar
{
    public static readonly Grammar eol = eof | str("\r\n", EoLToken) | ch('\n', EoLToken);

    public static readonly Grammar ws = match(c => c == ' ' || c == 't', WhitespaceToken).Option;

    public static readonly Grammar ws_only = line(@"^\s*$", WhitespaceToken);

    public static readonly Grammar header =
        (ws
        & ch('[', HeaderStartToken)
        & line(@"[^]]+", HeaderToken)
        & ch(']', HeaderEndToken)
        & ws
        & eol)
        + HeaderSyntax.New;

    public static readonly Grammar unparsable = ~header & line(".*", UnparsableToken);

    public static readonly Grammar comment =
        ws
        & (ch('#', CommentDelimiterToken) | ch(';', CommentDelimiterToken))
        & line(".*", CommentToken);

    public static readonly Grammar key = line(@"[^\s:=]+", KeyToken) + KeySyntax.New;

    public static readonly Grammar assign = ch('=', EqualsToken) | ch(':', ColonToken);

    public static readonly Grammar value = line(@"[^\s#;]+", ValueToken) + ValueSyntax.New;

    public static readonly Grammar kvp =
        (ws
        & key
        & ws
        & assign
        & ws
        & value
        & ws
        & comment.Option)
        + KeyValuePairSyntax.New;

    public static readonly Grammar single_line =
        (kvp
        | comment
        | ws_only
        | unparsable)
        & eol;

    public static readonly Grammar section = (header & single_line.Star) + SectionSyntax.New;

    public static readonly Grammar file = (single_line.Star & section.Star) + IniFileSyntax.New;
}

public static class TokenKind
{
    public static readonly string ValueToken = nameof(ValueToken);
    public static readonly string KeyToken = nameof(KeyToken);
    public static readonly string WhitespaceToken = nameof(WhitespaceToken);
    public static readonly string CommentDelimiterToken = nameof(CommentDelimiterToken);
    public static readonly string CommentToken = nameof(CommentToken);
    public static readonly string EoLToken = nameof(EoLToken);
    public static readonly string HeaderToken = nameof(HeaderToken);
    public static readonly string HeaderStartToken = nameof(HeaderStartToken);
    public static readonly string HeaderEndToken = nameof(HeaderEndToken);
    public static readonly string EqualsToken = nameof(EqualsToken);
    public static readonly string ColonToken = nameof(ColonToken);
    public static readonly string UnparsableToken = nameof(UnparsableToken);
}
