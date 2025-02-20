namespace Rules.MS_Build.Configure_CPM;

public class Reports
{
    [Test]
    public void project_without_CPM() => new ConfigureCentralPackageVersionManagement()
        .ForProject("NoCPM.cs")
        .HasIssue(Issue.WRN("Proj0800", "Define the <ManagePackageVersionsCentrally> node with the value 'true', or 'false'.")
        .WithSpan(00, 00, 00, 32));
}

public class Guards
{
    [Test]
    public void Projects_with_CPM_file() => new ConfigureCentralPackageVersionManagement()
       .ForProject("UseCPM.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_explicitly_without_CPM(string project) => new ConfigureCentralPackageVersionManagement()
        .ForProject(project)
        .HasNoIssues();
}
