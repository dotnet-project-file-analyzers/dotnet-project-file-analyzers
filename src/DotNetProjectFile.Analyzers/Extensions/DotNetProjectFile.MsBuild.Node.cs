using System.Text.RegularExpressions;

namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Provides extension methods for <see cref="Node"/> instances.
/// </summary>
public static class NodeExtensions
{
    private static readonly Regex WhitespaceRegex = new(@"\s+", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

    public static bool HasAnyCondition(this Node? node, params IEnumerable<string> conditions)
    {
        var conditionSet = new HashSet<string>(conditions.Select(c => NormalizeRegex(c)));

        var cur = node;

        while (cur != null)
        {
            var condition = NormalizeRegex(cur.Condition);

            if (IsAnyCondition(condition, conditionSet))
            {
                return true;
            }

            cur = cur.Parent;
        }

        return false;

        [return: NotNullIfNotNull(nameof(condition))]
        static string? NormalizeRegex(string? condition)
            => condition is null
            ? null
            : WhitespaceRegex.Replace(condition, string.Empty);
    }

    private static bool IsAnyCondition(string? condition, HashSet<string> conditions) 
        => condition is { }
        && conditions.Contains(condition);
}
