﻿using CodeAnalysis.TestTools.Contexts;
using Microsoft.CodeAnalysis;

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

        reported.Should().BeEquivalentTo(issues);
    }
}
