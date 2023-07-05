namespace Rules.Update_legacy_project;

public class Has_no_issues
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void For(string project)
        => new GuardUnsupported()
        .ForProject(project)
        .HasNoIssues();
}

public class Reports
{
    [Test]
    public void Legacy_project()
        => new GuardUnsupported()
        .ForProject("LegacyProject.cs")
        .HasIssue(new Issue("Proj0002", "Upgrade legacy .NET project file.").WithSpan(1, 1, 1, 2));
}
