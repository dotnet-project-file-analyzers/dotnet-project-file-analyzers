#pragma warning disable SA1300 // Element should begin with upper-case letter
// Grammar is commonly defined with lowercase words.

using DotNetProjectFile.Parsing.Internal;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.Parsing;

public partial class Grammar
{
    /// <summary>End of File.</summary>
    public static readonly Grammar eof = new EndOfFile();

    /// <summary>Lazy loads grammar.</summary>
    /// <remarks>
    /// Required for recursive patterns.
    /// </remarks>
    [Pure]
    public static Grammar Lazy(Func<Grammar> factory) => new Lazy(factory);

    /// <summary>Matches if the remaining source starts with the specified character.</summary>
    /// <param name="ch">
    /// The expected character.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    [Pure]
    public static Grammar ch(char ch, string? kind = default) => new StartWithChar(ch, kind);

    /// <summary>Matches if the remaining source starts with the specified string.</summary>
    /// <param name="str">
    /// The expected string.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    [Pure]
    public static Grammar str(string str, string? kind = default) => new StartWithString(str, kind);

    /// <inheritdoc cref="line(Regex, string)"/>
    [Pure]
    public static Grammar line([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, string? kind = default)
        => new RegularExpression(pattern, kind, true);

    /// <summary>Matches if the remaining source starts with the specified pattern.</summary>
    /// <param name="pattern">
    /// The expected pattern.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    /// <remarks>
    /// the pattern is only applied on the current line.
    /// </remarks>
    [Pure]
    public static Grammar line(Regex pattern, string? kind = default) => new RegularExpression(pattern, kind, true);

    /// <summary>Matches if the remaining source matches the predicate.</summary>
    /// <param name="predicate">
    /// The predicate to match.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    [Pure]
    public static Grammar match(Predicate<char> predicate, string? kind = default) => new Matches(predicate, kind);

    /// <inheritdoc cref="regex(Regex, string?)"/>
    [Pure]
    public static Grammar regex([StringSyntax(StringSyntaxAttribute.Regex)] string pattern, string? kind = default)
        => new RegularExpression(pattern, kind, false);

    /// <summary>Matches if the remaining source starts with the specified pattern.</summary>
    /// <param name="pattern">
    /// The expected pattern.
    /// </param>
    /// <param name="kind">
    /// The (optional) token kind.
    /// </param>
    /// <returns>
    /// A new grammar.
    /// </returns>
    [Pure]
    public static Grammar regex(Regex pattern, string? kind = default) => new RegularExpression(pattern, kind, false);
}
