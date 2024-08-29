namespace Rules.MS_Build.Use_CPMV;

public class Reports
{
    [Test]
    public void project_without_CPMV_file()
        => new UseCentralPackageVersionManagement()
        .ForProject("NoCPMV.cs")
        .HasIssue(new Issue("Proj0800", "The CPVM file 'Directory.Packages.props' could not be located.")
        .WithSpan(00, 00, 00, 32));
}

public class Guards
{
    [Test]
    public void Projects_with_CPMV_file()
        => new UseCentralPackageVersionManagement()
       .ForProject("UseCPMV.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_explicitly_without_CPMV(string project)
         => new UseCentralPackageVersionManagement()
        .ForProject(project)
        .HasNoIssues();
}
