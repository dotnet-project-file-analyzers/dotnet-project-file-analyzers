namespace Rules.MS_Build.Only_include_packages_with_explicit_license;

public class Reports
{
    [Test]
    public void on_packages_without_license_specified_in_nuspec() => new OnlyIncludePackagesWithExplicitLicense()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Qowaiv"" Version=""1.0.0.139"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0500", "The Qowaiv package is shipped without an explicitly defined license."));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new OnlyIncludePackagesWithExplicitLicense()
        .ForProject(project)
        .HasNoIssues();
}
