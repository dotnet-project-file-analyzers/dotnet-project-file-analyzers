namespace Rules.MS_Build.Use_Version_only_without_CPM;

public class Reports
{
    [Test]
    public void project_with_VersionOverride_without_CPM()
        => new UseVersionOnlyWithoutCpm()
        .ForProject("MisuseVersion.cs")
        .HasIssue(Issue.WRN("Proj0804", "Do not use Version when CPM is enabled")
        .WithSpan(07, 04, 07, 81));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_VersionOverride(string project)
         => new UseVersionOnlyWithoutCpm()
        .ForProject(project)
        .HasNoIssues();
}
