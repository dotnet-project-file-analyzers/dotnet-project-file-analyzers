using CodeAnalysis.TestTools.Contexts;
using System.Diagnostics;

namespace AwesomeAssertions;

internal static class FindsExtensions
{
    extension(ProjectAnalyzerVerifyContext context)
    {
        [DebuggerStepThrough, DebuggerHidden]
        public void HasNoIssues()
            => context.HasIssues();

        [DebuggerStepThrough, DebuggerHidden]
        public void HasIssue(Issue issue)
            => context.HasIssues(issue);
        
        [DebuggerStepThrough, DebuggerHidden]
        public void HasIssues(params Issue[] issues)
        {
            var diagnosics = Run.Sync(context.GetDiagnosticsAsync)
                .Where(d => !context.IgnoredDiagnostics.Contains(d.Id));

            diagnosics.Should().HaveIssues(issues);
        }
    }
}
