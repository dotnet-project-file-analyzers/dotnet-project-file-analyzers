namespace Rules.MS_Build.Licensing.Package_only_contains_deprecated_license_URL;

public class Reports
{
    [Test]
    public void on_packages_without_deprecated_nuspec() => new ThirdPartyLicenseResolver()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""SimpleInjector.Extensions.ExecutionContextScoping"" Version=""4.0.0"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0501", "The SimpleInjector.Extensions.ExecutionContextScoping package only contains a deprecated license URL."));
}

public class Guards
{
    [Test]
    public void license_urls() => new ThirdPartyLicenseResolver()
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
    public void Projects_without_issues(string project) => new ThirdPartyLicenseResolver()
        .ForProject(project)
        .HasNoIssues();
}
