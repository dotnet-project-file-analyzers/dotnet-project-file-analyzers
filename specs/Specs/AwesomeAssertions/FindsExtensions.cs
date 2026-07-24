using CodeAnalysis.TestTools.Contexts;
using System.Diagnostics;

namespace AwesomeAssertions;

internal static class FindsExtensions
{
    extension(ProjectAnalyzerVerifyContext context)
    {
        [DebuggerStepThrough]
        public void HasNoIssues()
            => context.HasIssues();

        [DebuggerStepThrough]
        public void HasIssue(Issue issue)
            => context.HasIssues(issue);
        
        [DebuggerStepThrough]
        public void HasIssues(params Issue[] issues)
        {
            var diagnosics = Run.Sync(context.GetDiagnosticsAsync)
                .Where(d => !context.IgnoredDiagnostics.Contains(d.Id));

            diagnosics.Should().HaveIssues(issues);
        }
    }
}
