namespace Rules.MS_Build.Licensing.Package_only_contains_deprecated_license_URL;

public class Reports
{
    [Test]
    public void on_packages_without_deprecated_nuspec() => new ThridPartyLicenseResolver()
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
    [Test, Ignore("Include a license URL that can be parsed")]
    public void license_urls() => new ThridPartyLicenseResolver()
      .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""MSTest.TestAdapter"" Version=""1.3.2"" />
  </ItemGroup>

</Project>")
      .HasNoIssues();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new ThridPartyLicenseResolver()
        .ForProject(project)
        .HasNoIssues();
}
