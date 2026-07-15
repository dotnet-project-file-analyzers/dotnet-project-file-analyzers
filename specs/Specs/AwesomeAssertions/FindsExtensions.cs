using CodeAnalysis.TestTools.Contexts;

namespace AwesomeAssertions;

internal static class FindsExtensions
{
    extension(ProjectAnalyzerVerifyContext context)
    {
        public void HasNoIssues()
            => context.HasIssues();

        public void HasIssue(Issue issue)
            => context.HasIssues(issue);

        public void HasIssues(params Issue[] issues)
        {
            var diagnosics = Run.Sync(context.GetDiagnosticsAsync)
                .Where(d => !context.IgnoredDiagnostics.Contains(d.Id));

            diagnosics.Should().HaveIssues(issues);
        }
    }
}
