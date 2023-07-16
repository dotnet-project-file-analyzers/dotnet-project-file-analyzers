namespace Rules.MS_Build.Add_additional_files;

public class Reports
{
    [Test]
    public void project_files_not_additional()
        => new AddAdditionalFile()
        .ForProject("EmptyProject.cs")
        .HasIssue(
            new Issue("Proj0006", "Add 'EmptyProject.csproj' to the additional files."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void project_files_as_additional(string project)
         => new AddAdditionalFile()
        .ForProject(project)
        .HasNoIssues();
}
