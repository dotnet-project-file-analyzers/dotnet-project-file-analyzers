namespace Rules.MS_Build.Use_VersionOverride_only_with_CPM;

public class Reports
{
    [Test]
    public void project_with_VersionOverride_without_CPM()
        => new UseVersionOverrideOnlyWithCpm()
        .ForProject("MisuseVersionOverride.cs")
        .HasIssue(Issue.WRN("Proj0803", "Use Version instead of VersionOverride when CPM is not enabled.")
        .WithSpan(07, 04, 07, 81));
}

public class Guards
{
    [Test]
    public void project_with_VersionOverride_with_CPM_enabled()
       => new UseVersionOverrideOnlyWithCpm()
       .ForProject("UseCPM.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_VersionOverride(string project)
         => new UseVersionOverrideOnlyWithCpm()
        .ForProject(project)
        .HasNoIssues();
}
