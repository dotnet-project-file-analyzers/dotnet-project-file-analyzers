using FluentAssertions.Collections;
using FluentAssertions.Execution;


namespace FluentAssertions;

internal sealed class DiagnosticsAssertions(IEnumerable<Diagnostic> actualValue)
    : GenericCollectionAssertions<Diagnostic>(actualValue, AssertionChain.GetOrCreate())
{
    public AndConstraint<DiagnosticsAssertions> HaveNoIssues() => HaveIssues();

    public AndConstraint<DiagnosticsAssertions> HaveIssue(Issue issue)
        => HaveIssues(issue);

    public AndConstraint<DiagnosticsAssertions> HaveIssues(params IEnumerable<Issue> issues)
    {
        var reported = Subject.Select(Issue.FromDiagnostic)
           .ToArray();

        var extra = reported.Except(issues, Issue.Comparer).ToArray();
        var missing = issues.Except(reported, Issue.Comparer).ToArray();
        var both = reported.Intersect(issues, Issue.Comparer).ToArray();

        if (extra.Any() || missing.Any())
        {
            var sb = new StringBuilder();
            sb.Append("Verification failed:");
            if (extra.Any()) sb.Append($" {extra.Length} extra");
            if (extra.Any() && missing.Any()) sb.Append(',');
            if (missing.Any()) sb.Append($" {missing.Length} missing");
            sb.AppendLine(".");
            foreach (var i in extra)
            {
                sb.AppendLine($"[+] {i}");
            }
            foreach (var i in missing)
            {
                sb.AppendLine($"[-] {i}");
            }
            foreach (var i in both)
            {
                sb.AppendLine($"[ ] {i}");
            }

            CurrentAssertionChain.FailWith(sb.ToString().Replace("{", "{{").Replace("}", "}}"));
        }

        return new(this);
    }
}
