using System.Text.RegularExpressions;

namespace DotNetProjectFile.MsBuild;

/// <summary>Evaluates MSBuild <c>Condition</c> expressions against a <see cref="PropertyRegistry"/>; returns <see langword="null"/> when unevaluable (unresolved property, unsupported syntax, malformed input).</summary>
/// <remarks>
/// Supports string literals (with <c>$(Property)</c> substitution), bare property references,
/// <c>==</c> / <c>!=</c>, <c>And</c> / <c>Or</c>, parentheses, and <c>Exists('path')</c>.
/// Numeric comparisons, negation, boolean literals as bare values, and functions other than
/// <c>Exists</c> yield <see langword="null"/>.
/// </remarks>
public static class ConditionEvaluator
{
    /// <summary>Evaluates <paramref name="condition"/> against <paramref name="registry"/>.</summary>
    [Pure]
    public static bool? Evaluate(string? condition, PropertyRegistry registry)
    {
        if (string.IsNullOrWhiteSpace(condition)) return null;

        var tokens = Tokenize(condition!);
        if (tokens is null) return null;

        var position = 0;
        var result = ParseDisjunction(tokens, ref position, registry);
        return position == tokens.Count ? result : null;
    }

    private static readonly Regex TokenPattern = new(
        @"\s+|('[^']*')|(\$\(\w+\))|(==)|(!=)|(\bAnd\b)|(\bOr\b)|([A-Za-z_]\w*)|([()])",
        RegexOptions.CultureInvariant | RegexOptions.Compiled | RegexOptions.IgnoreCase);

    [Pure]
    private static List<TokenInfo>? Tokenize(string source)
    {
        var tokens = new List<TokenInfo>();
        var position = 0;
        while (position < source.Length)
        {
            var match = TokenPattern.Match(source, position);
            if (!match.Success || match.Index != position) return null;
            position += match.Length;
            if (string.IsNullOrWhiteSpace(match.Value)) continue; // skip whitespace
            tokens.Add(Classify(match.Value));
        }
        return tokens;
    }

    [Pure]
    private static TokenInfo Classify(string text) => text switch
    {
        "==" => new(TokenKind.EqualEqual, text),
        "!=" => new(TokenKind.NotEqual, text),
        "(" => new(TokenKind.LeftParenthesis, text),
        ")" => new(TokenKind.RightParenthesis, text),
        _ when text.StartsWith("'") => new(TokenKind.StringLiteral, text),
        _ when text.StartsWith("$(") => new(TokenKind.PropertyReference, text),
        _ when text.Equals("And", StringComparison.OrdinalIgnoreCase) => new(TokenKind.And, text),
        _ when text.Equals("Or", StringComparison.OrdinalIgnoreCase) => new(TokenKind.Or, text),
        _ => new(TokenKind.Identifier, text),
    };

    [Pure]
    private static bool? ParseDisjunction(List<TokenInfo> tokens, ref int position, PropertyRegistry registry)
    {
        var left = ParseConjunction(tokens, ref position, registry);
        while (position < tokens.Count && tokens[position].Kind == TokenKind.Or)
        {
            position++;
            var right = ParseConjunction(tokens, ref position, registry);
            left = LogicalOr(left, right);
        }
        return left;
    }

    [Pure]
    private static bool? ParseConjunction(List<TokenInfo> tokens, ref int position, PropertyRegistry registry)
    {
        var left = ParseComparison(tokens, ref position, registry);
        while (position < tokens.Count && tokens[position].Kind == TokenKind.And)
        {
            position++;
            var right = ParseComparison(tokens, ref position, registry);
            left = LogicalAnd(left, right);
        }
        return left;
    }

    [Pure]
    private static bool? ParseComparison(List<TokenInfo> tokens, ref int position, PropertyRegistry registry)
    {
        if (position >= tokens.Count) return null;

        if (tokens[position].Kind == TokenKind.LeftParenthesis)
        {
            position++;
            var inner = ParseDisjunction(tokens, ref position, registry);
            if (position >= tokens.Count || tokens[position].Kind != TokenKind.RightParenthesis) return null;
            position++;
            return inner;
        }

        if (tokens[position].Kind == TokenKind.Identifier
            && position + 1 < tokens.Count
            && tokens[position + 1].Kind == TokenKind.LeftParenthesis)
        {
            return ParseFunctionCall(tokens, ref position, registry);
        }

        var leftValue = ParseValue(tokens, ref position, registry);

        if (position >= tokens.Count) return null;

        var op = tokens[position].Kind;
        if (op != TokenKind.EqualEqual && op != TokenKind.NotEqual) return null;
        position++;

        var rightValue = ParseValue(tokens, ref position, registry);

        if (leftValue is null || rightValue is null) return null;

        var equal = string.Equals(leftValue, rightValue, StringComparison.OrdinalIgnoreCase);
        return op == TokenKind.EqualEqual ? equal : !equal;
    }

    [Pure]
    private static bool? ParseFunctionCall(List<TokenInfo> tokens, ref int position, PropertyRegistry registry)
    {
        var name = tokens[position].Text;
        position++; // consume identifier
        position++; // consume '('
        if (position >= tokens.Count) return null;

        if (!string.Equals(name, "Exists", StringComparison.OrdinalIgnoreCase)) return null;

        var arg = ParseValue(tokens, ref position, registry);
        if (arg is null) return null;
        if (position >= tokens.Count || tokens[position].Kind != TokenKind.RightParenthesis) return null;
        position++;

        return !string.IsNullOrEmpty(arg)
            && (IOFile.Parse(arg).Exists || IODirectory.Parse(arg).Exists);
    }

    [Pure]
    private static string? ParseValue(List<TokenInfo> tokens, ref int position, PropertyRegistry registry)
    {
        if (position >= tokens.Count) return null;
        var token = tokens[position];
        if (token.Kind == TokenKind.StringLiteral)
        {
            position++;
            var inner = token.Text[1..^1];
            var (substituted, unresolved) = registry.Substitute(inner);
            return unresolved.Count == 0 ? substituted : null;
        }
        if (token.Kind == TokenKind.PropertyReference)
        {
            position++;
            var (substituted, unresolved) = registry.Substitute(token.Text);
            return unresolved.Count == 0 ? substituted : null;
        }
        return null;
    }

    [Pure]
    private static bool? LogicalAnd(bool? l, bool? r)
        => (l, r) switch
        {
            (false, _) => false,
            (_, false) => false,
            (true, true) => true,
            _ => null,
        };

    [Pure]
    private static bool? LogicalOr(bool? l, bool? r)
        => (l, r) switch
        {
            (true, _) => true,
            (_, true) => true,
            (false, false) => false,
            _ => null,
        };

    private enum TokenKind
    {
        StringLiteral,
        PropertyReference,
        Identifier,
        EqualEqual,
        NotEqual,
        And,
        Or,
        LeftParenthesis,
        RightParenthesis,
    }

    private readonly record struct TokenInfo(TokenKind Kind, string Text);
}
