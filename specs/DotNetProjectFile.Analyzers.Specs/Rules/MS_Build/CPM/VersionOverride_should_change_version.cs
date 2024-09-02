namespace Rules.MS_Build.VersionOverride_should_change_version;

public class Reports
{
    [Test]
    public void project_with_VersionOverride_without_CPM()
        => new VersionOverrideShouldChangeVersion()
        .ForProject("VersionUpdateNotDifferent.cs")
        .HasIssue(new Issue("Proj0806", "Remove VersionOverride or change it to a version different than defined by the CPM.")
        .WithSpan(08, 04, 08, 65));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_VersionOverride(string project)
         => new VersionOverrideShouldChangeVersion()
        .ForProject(project)
        .HasNoIssues();
}
