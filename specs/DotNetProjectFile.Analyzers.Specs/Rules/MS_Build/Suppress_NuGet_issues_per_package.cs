namespace Rules.MS_Build.Suppress_NuGet_issues_per_package;

public class Reports
{
    [Test]
    public void NoWarn_NU170x_in_PropertyGroup() => new SuppressNuGetViolationsPerCase().ForInlineCsproj($"""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <NoWarn>$(NoWarn);NU1701;NU1702;NU1703</NoWarn>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj1005", "Solve issue NU1701 or suppress it on the reported package").WithSpan(04, 04, 04, 51),
            Issue.WRN("Proj1005", "Solve issue NU1702 or suppress it on the reported package").WithSpan(04, 04, 04, 51),
            Issue.WRN("Proj1005", "Solve issue NU1703 or suppress it on the reported package").WithSpan(04, 04, 04, 51));

    [Test]
    public void NoWarn_NU190x_in_PropertyGroup() => new SuppressNuGetViolationsPerCase().ForInlineCsproj($"""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <NoWarn>$(NoWarn);NU1901;NU1902;NU1903;NU1904</NoWarn>
          </PropertyGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj1006", "Solve issue NU1901 or suppress it using <NuGetAuditSuppress>").WithSpan(04, 04, 04, 58),
            Issue.WRN("Proj1006", "Solve issue NU1902 or suppress it using <NuGetAuditSuppress>").WithSpan(04, 04, 04, 58),
            Issue.WRN("Proj1006", "Solve issue NU1903 or suppress it using <NuGetAuditSuppress>").WithSpan(04, 04, 04, 58),
            Issue.WRN("Proj1006", "Solve issue NU1904 or suppress it using <NuGetAuditSuppress>").WithSpan(04, 04, 04, 58));
}

public class Guards
{
    [Test]
    public void NoWarn_on_package_reference() => new SuppressNuGetViolationsPerCase().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
        	<PackageReference Include="Microsoft.IO.Redist" Version="6.1.3">
              <NoWarn>NU1701</NoWarn>
            </PackageReference>
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();
}
