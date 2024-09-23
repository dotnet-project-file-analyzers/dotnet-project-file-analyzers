using DotNetProjectFile.Analyzers.EditorConfig;

namespace Rules.EditorConfig.Globs_must_be_wellFormed;

public class Reports
{
    [Test]
    public void on_no_is_publishable()
        => new GlobsMustBeWellFormed()
        .ForProject("MalformedGlob.cs")
        .HasIssue(new Issue("Proj2201", "Define the <IsPublishable> node explicitly."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project)
         => new GlobsMustBeWellFormed()
        .ForProject(project)
        .HasNoIssues();
}
