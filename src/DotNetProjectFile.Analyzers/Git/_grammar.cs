using DotNetProjectFile.Parsing;
using static DotNetProjectFile.Git.TokenKind;

namespace DotNetProjectFile.Git;

internal sealed class GitIgnoreGrammar : Grammar
{
    public static readonly Grammar eol /*.........*/ = eof | str("\r\n", EoLToken) | ch('\n', EoLToken);
    public static readonly Grammar ws /*..........*/ = line(@"^\s*$", WhitespaceToken);
    public static readonly Grammar comment /*.....*/ = line("#.*$", CommentToken);
    public static readonly Grammar pattern /*.....*/ = line(".+$", PatternToken) + PatternSyntax.New;
    public static readonly Grammar single_line /*.*/ = (ws | comment | pattern) & eol;
    public static readonly Grammar file /*........*/ = single_line.Star + GitIgnoreSyntax.New;
}

internal static class TokenKind
{
    public static readonly string EoLToken = nameof(EoLToken);
    public static readonly string CommentToken = nameof(CommentToken);
    public static readonly string PatternToken = nameof(PatternToken);
    public static readonly string WhitespaceToken = nameof(WhitespaceToken);
}
