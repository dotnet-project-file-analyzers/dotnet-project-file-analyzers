using System.Text.RegularExpressions;

namespace DotNetProjectFile.MsBuild;

/// <summary>
/// Provides extension methods for <see cref="Node"/> instances.
/// </summary>
public static class NodeExtensions
{
    private static readonly Regex WhitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

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
        string? NormalizeRegex(string? condition)
        {
            if (condition is null)
            {
                return null;
            }

            return WhitespaceRegex.Replace(condition, string.Empty);
        }
    }

    private static bool IsAnyCondition(string? condition, HashSet<string> conditions)
    {
        if (condition is null)
        {
            return false;
        }

        return conditions.Contains(condition);
    }
}
