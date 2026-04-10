namespace Rules.MS_Build.Format_lines;

public class Reports
{
    [Test]
    public void on_missing_property() => new SeperateBlocksWithSingleWhiteLine()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <EnablePackageValidation>true</EnablePackageValidation>
              </PropertyGroup>
              <PropertyGroup>
                <ImplicitUsings>false</ImplicitUsings>
              </PropertyGroup>

              <!-- Comments should be ingored -->
              <ItemGroup>
                <PackageReference Update="DotNetProjectFile.Analyzers" Version="1.10.0" PrivateAssets="all" />
              </ItemGroup>

            </Project>
        """)
        .HasNoIssues();
}
