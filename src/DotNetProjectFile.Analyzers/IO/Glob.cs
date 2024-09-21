using Microsoft.CodeAnalysis.Text;
using System.Text.RegularExpressions;

namespace DotNetProjectFile.IO;

/// <summary>Represents a glob expression.</summary>
/// <remarks>
/// See: https://spec.editorconfig.org/index.html#glob-expressions.
/// </remarks>
public sealed class Glob
{
    private Glob(string expression, string pattern)
    {
        Expression = expression;
        Pattern = new(pattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100));
    }

    /// <summary>The expression.</summary>
    public string Expression { get; }

    /// <summary>The <see cref="Regex"/> equivalent to the expression.</summary>
    public Regex Pattern { get; }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Expression;

    /// <summary>Tries to parse the glob expression.</summary>
    /// <param name="expression">
    /// The expression to parse.
    /// </param>
    /// <returns>
    /// A <see cref="Glob"/> if the expression could be parsed, otherwise null.
    /// </returns>
    [Pure]
    public static Glob? TryParse(string? expression)
    {
        if (expression is not { Length: > 0 }) return null;

        var parser = GlobGrammar.glob.Parse(SourceText.From(expression));

        return parser.State == Parsing.Matching.EoF
            ? new(expression, parser.Syntax.ToString())
            : null;
    }
}
