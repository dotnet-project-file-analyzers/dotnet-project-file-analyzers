using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Grammr.Lexers;

/// <summary>Container with <see cref="Lexer"/> factory methods.</summary>
/// <remarks>
/// Best used with:
/// <code>
///  <![CDATA[
///  using static Grammr.Lexers.Shared;
///  ]]>
///  </code>.
/// </remarks>
public static class Shared
{
    /// <summary>Syntaxic sugar.</summary>
    /// <remarks>
    /// This allows:
    /// <code>
    /// <![CDATA[
    /// if (Chain
    ///     && reader.Emit(..)
    ///     && reader.Emit(..))
    /// ]]>
    /// </code>
    ///
    /// This should help with the outlining and therfor the readability of the code.
    /// </remarks>
    public const bool Chain = true;

    /// <summary>Syntaxic sugar.</summary>
    /// <remarks>
    /// This allows:
    /// <code>
    /// <![CDATA[
    /// if (Choise
    ///     || reader.Emit(..)
    ///     || reader.Emit(..))
    /// ]]>
    /// </code>
    ///
    /// This should help with the outlining and therfor the readability of the code.
    /// </remarks>
    public const bool Choise = false;

    /// <summary>Indicates the end of the stream.</summary>
    public static readonly Lexer eos = new EndOfStream();

    /// <summary>Indicates the end of a line.</summary>
    /// <remarks>
    /// Matches on:
    /// * \n
    /// * \r\n.
    /// </remarks>
    public static readonly Lexer eol = new EndOfLine();

    /// <summary>Indicates whitespace.</summary>
    /// <remarks>
    /// Matches on:
    /// * ' '
    /// * \t.
    /// </remarks>
    public static readonly Lexer ws = new Predicates(IsWhitespace, "Whitespace");

    /// <summary>Matches if the remaining source starts with the specified character.</summary>
    /// <param name="val">
    /// The expected character.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new lexer.
    /// </returns>
    public static Lexer ch(char val, [CallerMemberName] string? kind = null) => new Ch(val, kind);

    /// <summary>Matches if the remaining source starts with the specified characters.</summary>
    /// <param name="val">
    /// The expected characters.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new lexer.
    /// </returns>
    public static Lexer str(string val, [CallerMemberName] string? kind = null) => new Str(val, kind);

    /// <summary>Matches if the remaining source starts with a character in the specified range.</summary>
    /// <param name="range">
    /// The range (like "a-z").
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <remarks>
    /// Functional the second character in the string has no meaning, but it
    /// allows to read as range in the way you would normally specify it.
    /// </remarks>
    /// <returns>
    /// A new lexer.
    /// </returns>
    public static Lexer range(string range, [CallerMemberName] string? kind = null) => range.Length switch
    {
        3 when range[0] < range[2] && range[1] is '-'
            => new CharRange(range[0], range[2], kind),

        _ => throw new ArgumentOutOfRangeException(nameof(range), "Length must be two with the first character be before the second value."),
    };

    /// <summary>Matches if the remaining source starts with the specified pattern.</summary>
    /// <param name="pattern">
    /// The expected pattern.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new lexer.
    /// </returns>
    public static Lexer reg(
        [StringSyntax(StringSyntaxAttribute.Regex)] string pattern,
        [CallerMemberName] string? kind = null) => new Reg(pattern, kind);

    /// <inheritdoc cref="reg(Regex, string?)"/>
    public static Lexer reg(Regex pattern, [CallerMemberName] string? kind = null) => new Reg(pattern, kind);

    /// <summary>Matches if the remaining source matches the predicate.</summary>
    /// <param name="predicate">
    /// The predicate to match.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new lexer.
    /// </returns>
    public static Lexer match(Func<char, int, bool> predicate, [CallerMemberName] string? kind = null) => new Predicates(predicate, kind);

    /// <summary>Matches a line comment.</summary>
    /// <param name="start">
    /// The supported line comment starts sequences.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new lexer.
    /// </returns>
    public static Lexer line_comment(string start, string? kind = null) => new LineComment(start, kind);

    /// <summary>Matches a line.</summary>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new lexer.
    /// </returns>
    public static Lexer line(string? kind = null) => new Line(kind);

    private static bool IsWhitespace(char ch, int _) => ch is ' ' or '\t';
}
