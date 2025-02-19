namespace Rules.MS_Build.Licensing.Only_include_packages_with_explicit_license;

public class Reports
{
    [Test]
    public void on_packages_without_license_specified_in_nuspec() => new ThridPartyLicenseResolver()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.DotNet.PlatformAbstractions"" Version=""1.1.1"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0500", "The Microsoft.DotNet.PlatformAbstractions package is shipped without an explicitly defined license."));
}

public class Guards
{
    [Test]
    public void license_urls() => new ThridPartyLicenseResolver()
      .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.Azure.AppConfiguration.AspNetCore"" Version=""8.0.0"" />
  </ItemGroup>

</Project>")
      .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new ThridPartyLicenseResolver()
        .ForProject(project)
        .HasNoIssues();
}
