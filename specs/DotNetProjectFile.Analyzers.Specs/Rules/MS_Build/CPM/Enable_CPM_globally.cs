namespace Rules.MS_Build.Enable_CPM_globally;

public class Reports
{
    [Test]
    public void enabling_locally()
        => new EnableCentralPackageManagementCentrally()
        .ForProject("EnableCPMLocally.cs")
        .HasIssue(new Issue("Proj0802", "Enable <ManagePackageVersionsCentrally> in 'Directory.Packages.props' or a shared props file.")
        .WithSpan(04, 04, 04, 73));
}

public class Guards
{
    [Test]
    public void Projects_with_CPM_file()
        => new EnableCentralPackageManagementCentrally()
       .ForProject("UseCPM.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("NoCPM.cs")]
    public void Projects_explicitly_without_CPM(string project)
         => new EnableCentralPackageManagementCentrally()
        .ForProject(project)
        .HasNoIssues();
}
