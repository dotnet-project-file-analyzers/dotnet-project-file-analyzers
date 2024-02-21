using CodeAnalysis.TestTools.Contexts;
using FluentAssertions.Execution;

namespace FluentAssertions;

internal static class FindsExtensions
{
    public static void HasNoIssues(this ProjectAnalyzerVerifyContext context)
        => context.HasIssues();

    public static void HasIssue(this ProjectAnalyzerVerifyContext context, Issue issue)
        => context.HasIssues(issue);
    
    public static void HasIssues(this ProjectAnalyzerVerifyContext context, params Issue[] issues)
    {
        var reported = Run.Sync(context.GetDiagnosticsAsync)
            .Where(d => !context.IgnoredDiagnostics.Contains(d.Id))
            .Select(Issue.FromDiagnostic)
            .ToArray();

        var extra = reported.Except(issues).ToArray();
        var missing = issues.Except(reported).ToArray();
        var both = reported.Intersect(issues).ToArray();

        if(extra.Any() || missing.Any())
        {
            var sb = new StringBuilder();
            sb.Append("Verification failed:");
            if (extra.Any()) sb.Append($" {extra.Length} extra");
            if (extra.Any() && missing.Any()) sb.Append(',');
            if (missing.Any()) sb.Append($" {missing.Length} missing");
            sb.AppendLine(".");
            foreach(var i in extra)
            {
                sb.AppendLine($"[+] {i}");
            }
            foreach (var i in missing)
            {
                sb.AppendLine($"[-] {i}");
            }
            foreach(var i in both)
            {
                sb.AppendLine($"[ ] {i}");
            }

            Execute.Assertion.FailWith(sb.ToString());
        }
    }
}
