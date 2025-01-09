namespace Rules.MS_Build.Include_Directory_Packages_props;

public class Reports
{
    [Test]
    public void project_without_file() => new IncludeDirectoryPackagesProps()
        .ForProject("NoDirectoryPackagesProps.cs")
        .HasIssue(new Issue("Proj0801", "The file 'Directory.Packages.props' could not be located.")
        .WithSpan(00, 00, 00, 32));
}

public class Guards
{
    [Test]
    public void Projects_with_CPM_file() => new IncludeDirectoryPackagesProps()
       .ForProject("UseCPM.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_explicitly_without_CPM(string project) => new IncludeDirectoryPackagesProps()
        .ForProject(project)
        .HasNoIssues();
}
