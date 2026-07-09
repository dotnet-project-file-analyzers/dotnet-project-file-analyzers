using System.Diagnostics.CodeAnalysis;

namespace AwesomeAssertions;

internal sealed class IssueComparer : IEqualityComparer<Issue>
{
    public bool Equals(Issue? x, Issue? y) => (x, y) switch
    {
        (null, null) => true,
        (null, { }) => false,
        ({ }, null) => false,
        _ => x.Id == y.Id
            && y.Severity == y.Severity
            && x.Message == y.Message
            && (x.Span == default
                || y.Span == default
                || x.Span == y.Span)
                && (x.Path == default
                || y.Path == default
                || x.Path == y.Path),
    };
    public int GetHashCode([DisallowNull] Issue obj)
        => HashCode.Combine(obj.Id, obj.Severity, obj.Message);
}
