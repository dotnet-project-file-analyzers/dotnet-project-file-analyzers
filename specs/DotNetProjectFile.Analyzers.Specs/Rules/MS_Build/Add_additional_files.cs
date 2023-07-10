namespace Rules.MS_Build.Add_additional_files;

public class Reports
{
    [Test]
    public void project_files_not_additional()
        => new AddAdditionalFile()
        .ForProject("EmptyProject.cs")
        .HasIssue(
            new Issue("Proj0006", "Add 'EmptyProject.csproj' to the additional files.").WithSpan(0, 1, 0, 2));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantVB.vb")]
    public void project_files_as_additional(string project)
         => new AddAdditionalFile()
        .ForProject(project)
        .HasNoIssues();
}
