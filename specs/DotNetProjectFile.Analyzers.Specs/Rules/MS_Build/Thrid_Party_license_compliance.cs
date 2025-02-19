using DotNetProjectFile.NuGet;
using DotNetProjectFile.NuGet.Packaging;

namespace Rules.MS_Build.Thrid_Party_license_compliance;

public class Reports
{
    [Test]
    public void on_packages_without_license_specified_in_nuspec() => new ThirdPartyLicenseResolver()
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

    [Test]
    public void on_packages_with_deprecated_URL_in_nuspec() => new ThirdPartyLicenseResolver()
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

    [Test, Ignore("PackageLicenseExpression is not resolvable due to a test issue.")]
    public void on_package_with_license_incompatable_to_project() => new ThirdPartyLicenseResolver()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""SeeSharpTools.JY.GUI"" Version=""1.4.4.533"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0502", "The SeeSharpTools.JY.GUI package is distributed as GPL-3.0-only, which is imcompatable with the MIT license of the project."));
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

[Explicit]
public class Explore
{
    [Test]
    public void NuSpec_files()
    {
        var urls = PackageCache.GetDirectory().Files("/**/*.nuspec")!
            .Select(f =>
            {
                using var stream = f.TryOpenRead();
                try
                {
                    return NuSpecFile.Load(stream);
                }
                catch
                {
                    return null;
                }
            })
            .OfType<NuSpecFile>();

        foreach (var url in urls)
        {
#pragma warning disable RS1035 // Do not use APIs banned for analyzers
            Console.WriteLine($"{url.Metadata.Id} v{url.Metadata.Version}: {url.Metadata.License}");
#pragma warning restore RS1035 // Do not use APIs banned for analyzers
        }
    }
}
