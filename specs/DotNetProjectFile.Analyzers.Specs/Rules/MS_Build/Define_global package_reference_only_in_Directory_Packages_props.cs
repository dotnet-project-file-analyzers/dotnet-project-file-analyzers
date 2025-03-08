namespace Rules.MS_Build.Define_global_package_reference_only_in_Directory_Packages_props;

public class Reports
{
    [Test]
    public void on_global_package_references_in_csproj() => new DefineGlobalPackageReferenceInDirectoryPackagesOnly()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <GlobalPackageReference Include=""DotNetProjectFile.Analyzers"" Version=""1.5.8"" />
  </ItemGroup>

</Project>")
        .HasIssue(
            Issue.WRN("Proj0808", "The <GlobalPackageReference> should be defined in the Directory.Packages.props").WithSpan(07, 04, 07, 84));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("UseCPM.cs")]
    public void Projects_without_issues(string project) => new DefineGlobalPackageReferenceInDirectoryPackagesOnly()
        .ForProject(project)
        .HasNoIssues();
}
