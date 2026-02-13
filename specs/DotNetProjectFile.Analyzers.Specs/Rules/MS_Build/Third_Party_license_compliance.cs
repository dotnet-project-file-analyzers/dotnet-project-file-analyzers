using DotNetProjectFile.NuGet;
using DotNetProjectFile.NuGet.Packaging;

namespace Rules.MS_Build.Third_Party_license_compliance;

public class Reports
{
    [Test]
    public void on_packages_without_license_specified_in_nuspec() => new ThirdPartyLicenseResolver()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.DotNet.PlatformAbstractions"" Version=""1.1.1"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0500", "The Microsoft.DotNet.PlatformAbstractions (1.1.1) package is shipped without an explicitly defined license"));

    [Test]
    public void on_packages_with_deprecated_URL_in_nuspec() => new ThirdPartyLicenseResolver()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""SimpleInjector.Extensions.ExecutionContextScoping"" Version=""4.0.0"" />
    <PackageReference Include=""Testcontainers"" Version=""3.10.0"" />
  </ItemGroup>

</Project>")
       .HasIssues(
            Issue.WRN("Proj0501", "The SimpleInjector ([4.0.0,5.0)) transitive package in SimpleInjector.Extensions.ExecutionContextScoping only contains a deprecated 'https://simpleinjector.org/license' license URL"),
            Issue.WRN("Proj0501", "The SimpleInjector.Extensions.ExecutionContextScoping (4.0.0) package only contains a deprecated 'https://simpleinjector.org/license' license URL"),
            Issue.WRN("Proj0501", "The SshNet.Security.Cryptography ([1.3.0]) transitive package in Testcontainers only contains a deprecated 'https://github.com/sshnet/Cryptography/blob/master/LICENSE' license URL"));

    [Test]
    public void on_package_with_license_incompatable_to_project() => new ThirdPartyLicenseResolver()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""SeeSharpTools.JY.GUI"" Version=""1.4.4.533"" />
  </ItemGroup>

</Project>")
        .HasIssue(Issue.WRN("Proj0502", "The SeeSharpTools.JY.GUI (1.4.4.533) package is distributed as GPL-3.0-only, which is imcompatable with the NOASSERTION license of the project")
        .WithSpan(08, 04, 08, 75));

    [Test]
    public void on_package_with_unknown_custom_license() => new ThirdPartyLicenseResolver()
       .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""SonarAnalyzer.CSharp"" Version=""10.6.0.109712"" />
  </ItemGroup>

</Project>")
        .HasIssue(Issue.WRN("Proj0503", @"Add <ThirdPartyLicense Include=""SonarAnalyzer.CSharp"" Hash=""IBM9yngU7omFyJOMSFSy0w"" /> to accept the license")
        .WithSpan(07, 04, 07, 79));

    [Test]
    public void on_package_with_changed_custom_license() => new ThirdPartyLicenseResolver()
        .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""SonarAnalyzer.CSharp"" Version=""10.6.0.109712"" />
  </ItemGroup>

  <ItemGroup Label=""Custom licenses"">
    <ThirdPartyLicense Include=""SonarAnalyzer.CSharp"" Hash=""TESLAngU7omFyJOMSFSy0w"" />
  </ItemGroup>

</Project>")
        .HasIssue(Issue.WRN("Proj0504", "The license for SonarAnalyzer.CSharp has changed as its hash is now IBM9yngU7omFyJOMSFSy0w")
        .WithSpan(11, 04, 11, 86));

    [Test]
    public void missing_include() => new ThirdPartyLicenseCompliance()
     .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <ThirdPartyLicense Update="Qowaiv" Hash="QED9yngU7o+Fy128_FSy0w" />
          </ItemGroup>

        </Project>
        """)
     .HasIssue(Issue.ERR("Proj0505", "Include has not been specified")
     .WithSpan(07, 04, 07, 71));

    [Test]
    public void invalid_glob() => new ThirdPartyLicenseCompliance()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <ThirdPartyLicense Include="Qowaiv[" Hash="QED9yngU7o+Fy128_FSy0w" />
          </ItemGroup>

        </Project>
        """)
       .HasIssue(Issue.ERR("Proj0505", "Include is not valid GLOB pattern")
       .WithSpan(07, 04, 07, 73));

    [Test]
    public void missing_hash() => new ThirdPartyLicenseCompliance()
       .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <ThirdPartyLicense Include="Qowaiv" />
          </ItemGroup>

        </Project>
        """)
       .HasIssue(Issue.ERR("Proj0506", "Hash has not been specified")
       .WithSpan(07, 04, 07, 42));

    [TestCase("QED9yngU7o-Fy128_FSy0")] //   Too short
    [TestCase("QED9yngU7o-Fy128_FSy023")] // Too long
    [TestCase("QED9yngU7o%4Fy128FSy02")] //  Invalid character
    public void invalid_hash(string hash) => new ThirdPartyLicenseCompliance()
       .ForInlineCsproj($"""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <ThirdPartyLicense Include="Qowaiv" Hash="${hash}" />
          </ItemGroup>

        </Project>
        """)
       .HasIssue(Issue.ERR("Proj0506", "Hash is not valid")
       .WithSpan(07, 04, 07, 51 + hash.Length));

    [Test]
    public void conditional() => new ThirdPartyLicenseCompliance()
       .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

            <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            </PropertyGroup>

            <ItemGroup Condition="$(TargetFramework) == 'net8.0'">
            <ThirdPartyLicense Include="Qowaiv" Hash="QED9yngU7o+Fy128_FSy0w" />
            </ItemGroup>

        </Project>
        """)
       .HasIssue(Issue.ERR("Proj0507", "The <ThirdPartyLicense> can not be conditional")
       .WithSpan(07, 04, 07, 72));

    [Test]
    public void transient_dependencies_with_custom_license() => new ThirdPartyLicenseResolver().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="MongoDB.Driver.Core" Version="2.30.0" />
          </ItemGroup>

          <ItemGroup Label="Custom licenses">
            <ThirdPartyLicense Include="MongoDB.Libmongocrypt" Hash="H+71D1Qif9a+jKUWZKrMdQ" />
            <ThirdPartyLicense Include="Snappier" Hash="v2I091CLKicpyApWDF77iQ" />
          </ItemGroup>

        </Project>
        """)
   .HasIssues(
       Issue.WRN("Proj0500", "The SharpCompress (0.30.1) transitive package in MongoDB.Driver.Core is shipped without an explicitly defined license"),
       Issue.WRN("Proj0501", "The AWSSDK.Core ([3.7.100.14, 4.0.0)) transitive package in MongoDB.Driver.Core only contains a deprecated 'http://aws.amazon.com/apache2.0/' license URL"),
       Issue.WRN("Proj0501", "The AWSSDK.SecurityToken (3.7.100.14) transitive package in MongoDB.Driver.Core only contains a deprecated 'http://aws.amazon.com/apache2.0/' license URL"));

}

public class Guards
{
    [Test]
    public void license_urls() => new ThirdPartyLicenseResolver()
      .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""Microsoft.Azure.AppConfiguration.AspNetCore"" Version=""8.0.0"" />
  </ItemGroup>

</Project>")
      .HasNoIssues();

    [Test]
    public void registered_custom_licenses() => new ThirdPartyLicenseResolver()
      .ForInlineCsproj(@"
<Project Sdk=""Microsoft.NET.Sdk"">

    <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    </PropertyGroup>

  <ItemGroup>
    <PackageReference Include=""SonarAnalyzer.CSharp"" Version=""10.6.0.109712"" />
  </ItemGroup>

  <ItemGroup Label=""Custom licenses"">
    <ThirdPartyLicense Include=""SonarAnalyzer.*"" Hash=""IBM9yngU7omFyJOMSFSy0w"" />
  </ItemGroup>

</Project>")
      .HasNoIssues();

    [Test]
    public void unconditional_fully_specified_node() => new ThirdPartyLicenseCompliance()
        .ForInlineCsproj(@$"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ThirdPartyLicense Include=""Qowaiv"" Hash=""QED9yngU7o+Fy128_FSy0w"" />
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
            Console.WriteLine($"{url.Metadata.Id} v{url.Metadata.Version}: {url.Metadata.License}");
        }
    }
}
