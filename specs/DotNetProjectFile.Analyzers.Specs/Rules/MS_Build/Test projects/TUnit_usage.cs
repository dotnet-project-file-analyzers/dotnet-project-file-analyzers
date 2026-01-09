namespace Rules.MS_Build.Test_projects.TUnit_usage;

public class Reports
{
    [Test]
    public void on_projects_not_be_an_executable() => new TUnitUsage()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <OutputType>library</OutputType>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup Label=""Build tools"">
    <PackageReference Include=""TUnit"" Version=""0.13.20"" PrivateAssets=""all"" />
  </ItemGroup>

</Project>")
        .HasIssue(Issue.WRN("Proj1103", "Set <OutputType> to exe to run the TUnit tests")
        .WithSpan(04, 04, 04, 36));

    [Test]
    public void on_projects_without_output_type() => new TUnitUsage()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup Label=""Build tools"">
    <PackageReference Include=""TUnit.Engine"" Version=""0.13.20"" PrivateAssets=""all"" />
  </ItemGroup>

</Project>")
        .HasIssue(Issue.WRN("Proj1103", "Set <OutputType> to exe to run the TUnit tests")
        .WithSpan(00, 00, 00, 32));

    [Test]
    public void on_projects_with_test_SDK() => new TUnitUsage()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <OutputType>exe</OutputType>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup Label=""Build tools"">
    <PackageReference Include=""Microsoft.NET.Test.Sdk"" Version=""17.13.0"" PrivateAssets=""all"" />    
    <PackageReference Include=""TUnit"" Version=""0.13.20"" PrivateAssets=""all"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj1104", "Remove either TUnit or Microsoft.NET.Test.Sdk as they can not be combined")
       .WithSpan(09, 04, 09, 95));
}

public class Guards
{
    [TestCase("TestProject.cs")]
    [TestCase("TUnitTestProject.cs")]
    public void test_project(string project) => new TUnitUsage()
        .ForProject(project)
        .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void non_test_project(string project) => new TUnitUsage()
        .ForProject(project)
        .HasNoIssues();
}
