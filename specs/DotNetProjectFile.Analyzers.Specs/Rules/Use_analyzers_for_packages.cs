using CodeAnalysis.TestTools;
using DotNetProjectFile.Analyzers;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Rules.Use_analyzers_for_packages;

#if RELEASE
[TestFixture(Ignore = "Build has difficulties resolving (some) NuGet packages")]
#endif
public class Reports
{
    [Test]
    public void missing_analyzers()
        => new UseAnalyzersForPackages()
        .ForProject("PackagesWithoutAnalyzers.cs")
        .HasIssues(
            new("Proj1001", "Use FluentAssertions.Analyzers to analyze FluentAssertions."),
            new("Proj1001", "Use Microsoft.AspNetCore.Components.Analyzers to analyze Microsoft.AspNetCore.Antiforgery."),
            new("Proj1001", "Use Microsoft.Azure.Functions.Analyzers to analyze Microsoft.Azure.Functions.Extensions."),
            new("Proj1001", "Use Microsoft.EntityFrameworkCore.Analyzers to analyze Microsoft.EntityFrameworkCore.Abstractions."),
            new("Proj1001", "Use MongoDB.Analyzer to analyze MongoDB.Bson."),
            new("Proj1001", "Use NUnit.Analyzers to analyze nunit.framework."),
            new("Proj1001", "Use Qowaiv.Analyzers.CSharp to analyze Qowaiv."),
            new("Proj1001", "Use SerilogAnalyzer to analyze Serilog."),
            new("Proj1001", "Use xunit.analyzers to analyze xunit.abstractions."));
}

#if RELEASE
[TestFixture(Ignore = "Build has difficulties resolving (some) NuGet packages")]
#endif
public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    [TestCase("PackagesWithAnalyzers.cs")]
    public void Projects_without_issues(string project)
         => new UseAnalyzersForPackages()
        .ForProject(project)
        .HasNoIssues();
}
