namespace Rules.MS_Build.Configure_CPVM;

public class Reports
{
    [Test]
    public void project_without_CPVM()
        => new ConfigureCentralPackageVersionManagement()
        .ForProject("NoCPVM.cs")
        .HasIssue(new Issue("Proj0800", "Define the <ManagePackageVersionsCentrally> node with the value 'true', or 'false'.")
        .WithSpan(00, 00, 00, 32));
}

public class Guards
{
    [Test]
    public void Projects_with_CPVM_file()
        => new ConfigureCentralPackageVersionManagement()
       .ForProject("UseCPVM.cs")
       .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_explicitly_without_CPVM(string project)
         => new ConfigureCentralPackageVersionManagement()
        .ForProject(project)
        .HasNoIssues();
}
