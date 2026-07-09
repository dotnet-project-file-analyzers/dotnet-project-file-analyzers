namespace Rules.MS_Build.Project_file_could_not_be_located;

public class Has_no_issues
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("NoAdditionalFiles.cs")]
    public void For(string project)
        => new GuardUnsupported()
        .ForProject(project)
        .HasNoIssues();
}
