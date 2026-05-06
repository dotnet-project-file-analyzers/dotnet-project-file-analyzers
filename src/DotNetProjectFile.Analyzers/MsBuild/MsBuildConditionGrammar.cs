using Grammr;

namespace DotNetProjectFile.MsBuild;

/// <summary>Lexer for MSBuild <c>Condition</c> expressions, built on dpfa's <see cref="Grammar"/>.</summary>
/// <remarks>
/// Order matters when composing these tokens into alternation: longer operator prefixes
/// (<c>==</c>, <c>!=</c>, <c>&lt;=</c>, <c>&gt;=</c>) must precede their single-character
/// counterparts so the longer lexeme wins.
/// </remarks>
public sealed class MsBuildConditionGrammar : Grammar
{
    /// <summary>One or more whitespace characters. Conditions are whitespace-tolerant.</summary>
    public static readonly Token WhiteSpace = regex(@"\s+");

    /// <summary>Single-quoted string literal; MSBuild does not support escape sequences.</summary>
    public static readonly Token StringLiteral = regex(@"'[^']*'");

    /// <summary>An MSBuild property reference of the form <c>$(Name)</c>.</summary>
    public static readonly Token PropertyReference = regex(@"\$\(\w+\)");

    /// <summary>Identifier covering function names, boolean literals, and logical operators; disambiguated at the parser level.</summary>
    public static readonly Token Identifier = regex(@"[A-Za-z_][A-Za-z0-9_]*");

    /// <summary>Equality comparison <c>==</c>.</summary>
    public static readonly Token EqualEqual = str("==");

    /// <summary>Inequality comparison <c>!=</c>. Must be matched before <see cref="ExclamationMark"/>.</summary>
    public static readonly Token NotEqual = str("!=");

    /// <summary>Less-than-or-equal comparison <c>&lt;=</c>. Must be matched before <see cref="Less"/>.</summary>
    public static readonly Token LessOrEqual = str("<=");

    /// <summary>Greater-than-or-equal comparison <c>&gt;=</c>. Must be matched before <see cref="Greater"/>.</summary>
    public static readonly Token GreaterOrEqual = str(">=");

    /// <summary>Less-than comparison <c>&lt;</c>.</summary>
    public static readonly Token Less = ch('<');

    /// <summary>Greater-than comparison <c>&gt;</c>.</summary>
    public static readonly Token Greater = ch('>');

    /// <summary>Left parenthesis <c>(</c>.</summary>
    public static readonly Token LeftParenthesis = ch('(');

    /// <summary>Right parenthesis <c>)</c>.</summary>
    public static readonly Token RightParenthesis = ch(')');

    /// <summary>Comma <c>,</c>. Used as argument separator in function calls.</summary>
    public static readonly Token Comma = ch(',');

    /// <summary>Logical-not / negation <c>!</c>.</summary>
    public static readonly Token ExclamationMark = ch('!');
}
