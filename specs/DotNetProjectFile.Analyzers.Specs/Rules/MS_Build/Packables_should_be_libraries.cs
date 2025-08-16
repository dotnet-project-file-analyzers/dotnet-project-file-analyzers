namespace Rules.MS_Build.Packables_should_be_libraries;

public class Reports
{
    [Test]
    public void on_not_alphabetical_order() => new PackablesShouldBeLibraries()
       .ForInlineCsproj("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <OutputType>exe</OutputType>
    <IsPackable>true</IsPackable>
  </PropertyGroup>

</Project>
""")
        .HasIssues(
            Issue.ERR("CS5001", "Program does not contain a static 'Main' method suitable for an entry point"),
            Issue.WRN("Proj0219", "The <IsPackable> node is set to 'true' but the <OutputType> is not 'library'").WithSpan(0, 0, 0, 32));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new OrderPackageVersionsAlphabetically()
        .ForProject(project)
        .HasNoIssues();
}
