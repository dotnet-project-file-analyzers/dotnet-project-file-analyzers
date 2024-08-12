namespace Rules.MS_Build.Update_legacy_project;

public class Has_no_issues
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void For(string project)
        => new GuardUnsupported()
        .ForProject(project)
        .HasNoIssues();
}

#if Is_Windows
public class Reports
{
    [Test]
    public void Legacy_project()
        => new GuardUnsupported()
        .ForProject("LegacyProject.cs")
        .HasIssue(new Issue("Proj0002", "Upgrade legacy MS Build project file.").WithSpan(1, 1, 1, 2));
}
#endif
