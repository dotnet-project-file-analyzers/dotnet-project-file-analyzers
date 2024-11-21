using CodeAnalysis.TestTools.Contexts;

namespace FluentAssertions;

internal static class FindsExtensions
{
    public static void HasNoIssues(this ProjectAnalyzerVerifyContext context)
        => context.HasIssues();

    public static void HasIssue(this ProjectAnalyzerVerifyContext context, Issue issue)
        => context.HasIssues(issue);
    
    public static void HasIssues(this ProjectAnalyzerVerifyContext context, params Issue[] issues)
    {
        var diagnosics = Run.Sync(context.GetDiagnosticsAsync)
            .Where(d => !context.IgnoredDiagnostics.Contains(d.Id));

        diagnosics.Should().HaveIssues(issues);
    }
}
