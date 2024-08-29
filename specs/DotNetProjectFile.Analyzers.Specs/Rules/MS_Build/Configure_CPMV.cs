namespace Rules.MS_Build.Configure_CPMV;

public class Reports
{
    [Test]
    public void project_without_CPMV()
        => new ConfigureCentralPackageVersionManagement()
        .ForProject("NoCPMV.cs")
        .HasIssue(new Issue("Proj0800", "Define the <ManagePackageVersionsCentrally> node with the value 'true', or 'false'.")
        .WithSpan(00, 00, 00, 32));
}

public class Guards
{
    [Test]
    public void Projects_with_CPMV_file()
        => new ConfigureCentralPackageVersionManagement()
       .ForProject("UseCPMV.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_explicitly_without_CPMV(string project)
         => new ConfigureCentralPackageVersionManagement()
        .ForProject(project)
        .HasNoIssues();
}
