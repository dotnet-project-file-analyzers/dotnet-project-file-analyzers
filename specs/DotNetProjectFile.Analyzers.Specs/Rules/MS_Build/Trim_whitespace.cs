namespace Specs.Rules.MS_Build.Trim_whitespace;

public class Reports
{
    [Test]
    public void whitespace_around_attribute_assignment() => new TrimWhitespace()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include  = "Qowaiv" Version="7.4.7" />
          </ItemGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj3003", "Remove leading whitespace").WithSpan(07, 29, 07, 31),
            Issue.WRN("Proj3003", "Remove trailing whitespace").WithSpan(7, 32, 07, 33));

    [Test]
    public void whitespace_in_attribute_value() => new TrimWhitespace()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include=" Qowaiv  " Version="7.4.7" />
          </ItemGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj3003", "Remove leading whitespace").WithSpan(07, 31, 07, 32),
            Issue.WRN("Proj3003", "Remove trailing whitespace").WithSpan(7, 38, 07, 40));
}
