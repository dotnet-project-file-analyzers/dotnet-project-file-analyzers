namespace Specs.Rules.MS_Build.ThirdPartyLicensence_compliance;

public class Reports
{
    [Test]
    public void missing_include() => new ThirdPartyLicenseCompliance()
       .ForInlineCsproj(@$"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ThirdPartyLicense Update=""Qowaiv"" Hash=""QED9yngU7o-Fy128_FSy0w"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0505", "Include has not been specified")
       .WithSpan(07, 04, 07, 71));

    [Test]
    public void invalid_glob() => new ThirdPartyLicenseCompliance()
       .ForInlineCsproj(@$"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ThirdPartyLicense Include=""Qowaiv["" Hash=""QED9yngU7o-Fy128_FSy0w"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0505", "Include is not valid GLOB pattern")
       .WithSpan(07, 04, 07, 73));

    [Test]
    public void missing_hash() => new ThirdPartyLicenseCompliance()
       .ForInlineCsproj(@$"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ThirdPartyLicense Include=""Qowaiv"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0506", "Hash has not been specified")
       .WithSpan(07, 04, 07, 42));

    [TestCase("QED9yngU7o-Fy128_FSy0")] //   Too short
    [TestCase("QED9yngU7o-Fy128_FSy023")] // Too long
    [TestCase("QED9yngU7o%4Fy128FSy02")] //  Invalid character
    public void invalid_hash(string hash) => new ThirdPartyLicenseCompliance()
       .ForInlineCsproj(@$"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ThirdPartyLicense Include=""Qowaiv"" Hash=""${hash}"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0506", "Hash is not valid")
       .WithSpan(07, 04, 07, 51 + hash.Length));

    [Test]
    public void condtional() => new ThirdPartyLicenseCompliance()
       .ForInlineCsproj(@$"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup Condition=""$(TargetFramework) == 'net8.0'"">
    <ThirdPartyLicense Include=""Qowaiv"" Hash=""QED9yngU7o-Fy128_FSy0w"" />
  </ItemGroup>

</Project>")
       .HasIssue(Issue.WRN("Proj0507", "The <ThirdPartyLicense> can not be conditional")
       .WithSpan(07, 04, 07, 72));

    public class Guards
    {
        [Test]
        public void unconditonal_fully_specified_node() => new ThirdPartyLicenseCompliance()
            .ForInlineCsproj(@$"
<Project Sdk=""Microsoft.NET.Sdk"">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <ThirdPartyLicense Include=""Qowaiv"" Hash=""QED9yngU7o-Fy128_FSy0w"" />
  </ItemGroup>

</Project>")
            .HasNoIssues();

        [TestCase("CompliantCSharp.cs")]
        [TestCase("CompliantCSharpPackage.cs")]
        public void Projects_without_issues(string project) => new ThirdPartyLicenseCompliance()
            .ForProject(project)
            .HasNoIssues();
    }
}
