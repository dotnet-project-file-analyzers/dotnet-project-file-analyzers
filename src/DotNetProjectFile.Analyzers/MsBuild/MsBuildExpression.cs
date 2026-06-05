using System.Text.RegularExpressions;

namespace DotNetProjectFile.MsBuild;

public readonly struct MsBuildExpression
{
    public static readonly MsBuildExpression Empty;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string Expression;

    private MsBuildExpression(string name) => Expression = name;

    public bool HasValue => Expression is not null;

    /// <inheritdoc />
    public override string ToString() => Expression is null ? string.Empty : $"$({Expression})";

    public static MsBuildExpression TryParse(string str)
        => ParseAll(str).FirstOrNone() is { } first
        && first.Expression.Length == str.Trim().Length - 3
        ? first
        : Empty;

    public static IEnumerable<MsBuildExpression> ParseAll(string str)
    {
        const int NONE = -1;
        var start = NONE;
        var level = 0;
        for (var i = 0; i < str.Length; i++)
        {
            var ch = str[i];

            // A $ not followed by a (
            if (start is not NONE && i - start is 1 && ch is not '(')
            {
                start = NONE;
            }
            else if (ch is '$')
            {
                start = start is NONE ? i : start;
            }
            else if (ch is '(')
            {
                level++;
            }
            else if (ch is ')')
            {
                level--;
                if (level is 0 && start is not NONE)
                {
                    if (i - start > 2)
                    {
                        yield return new(str[(start + 2)..i]);
                    }
                    start = NONE;
                }
            }
        }
    }
}
