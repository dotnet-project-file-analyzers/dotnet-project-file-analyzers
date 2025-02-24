namespace Rules.RESX.Add_additional_files;

public class Reports
{
    [Test]
    public void project_files_not_additional() => new Resx.AddAdditionalFile()
        .ForProject("ResxUnsorted.cs")
        .HasIssue(Issue.WRN("Proj0006", "Add 'Resources.resx' to the additional files")
        .WithPath("Resources.resx"));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void project_files_as_additional(string project) => new Resx.AddAdditionalFile()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void Directory_Build_props_not_being_added() => new Resx.AddAdditionalFile()
        .ForProject("WithDirectoryBuildProps.cs")
        .HasNoIssues();
}
