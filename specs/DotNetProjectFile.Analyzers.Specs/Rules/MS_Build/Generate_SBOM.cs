namespace Rules.MS_Build.Generate_SBOM;

public class Reports
{
    [Test]
    public void on_missing_property() => new GenerateSbom()
        .ForProject("PackageWithoutSBOM.cs")
        .HasIssue(Issue.WRN("Proj0243", "Enable SBOM generation with <GenerateSBOM> is 'true' or define the <IsPackable> node with value 'false'")
        .WithSpan(00, 00, 00, 32));


    [Test]
    public void on_disabled_property() => new GenerateSbom()
        .ForProject("PackageWithSBOMDisabled.cs")
        .HasIssue(Issue.WRN("Proj0243", "Enable SBOM generation with <GenerateSBOM> is 'true' or define the <IsPackable> node with value 'false'")
        .WithSpan(05, 04, 05, 38));

    [Test]
    public void on_missing_reference() => new GenerateSbom()
        .ForProject("PackageWithoutSBOMReference.cs")
        .HasIssue(Issue.WRN("Proj0243", "Register the NuGet package 'Microsoft.Sbom.Targets' or define the <IsPackable> node with value 'false'")
        .WithSpan(05, 04, 05, 37));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new GenerateSbom()
        .ForProject(project)
        .HasNoIssues();

    [TestCase]
    public void global_package_reference() => new GenerateSbom().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <GenerateSBOM>true</GenerateSBOM>
          </PropertyGroup>

          <ItemGroup>
            <GlobalPackageReference Include="Microsoft.Sbom.Targets" Version="4.1.5" />
          </ItemGroup>

        </Project>
        """)
        .HasNoIssues();
}

public class Reports_when_sbom_only_registered_inside_false_condition
{
    [Test]
    public void Sbom_package_inside_false_condition_is_treated_as_unregistered() => new GenerateSbom()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <IsPackable>true</IsPackable>
                <TargetFramework>net10.0</TargetFramework>
                <GenerateSBOM>true</GenerateSBOM>
              </PropertyGroup>

              <ItemGroup Condition="'a' == 'b'">
                <PackageReference Include="Microsoft.Sbom.Targets" Version="4.1.5" />
              </ItemGroup>

            </Project>
            """)
        .HasIssue(Issue
            .WRN("Proj0243", "Register the NuGet package 'Microsoft.Sbom.Targets' or define the <IsPackable> node with value 'false'"));
}
