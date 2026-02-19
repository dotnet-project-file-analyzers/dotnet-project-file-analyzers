namespace Rules.MS_Build.Define_CPM_related_config_in_Directory_Packages_props;

public class Reports
{
    [Test]
    public void on_global_package_references_in_csproj() => new DefineCpmRelatedConfigInDirectoryPackages()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <CentralPackageFloatingVersionsEnabled>false</CentralPackageFloatingVersionsEnabled>
                <CentralPackageTransitivePinningEnabled>false</CentralPackageTransitivePinningEnabled>
                <CentralPackageVersionOverrideEnabled>false</CentralPackageVersionOverrideEnabled>
              </PropertyGroup>

              <ItemGroup>
                <GlobalPackageReference Include="DotNetProjectFile.Analyzers" Version="1.8.0" />
                <PackageVersion Include="Qowaiv" Version="7.4.7" />
              </ItemGroup>

            </Project>
            """)
        .HasIssues(
            Issue.WRN("Proj0808", "The <CentralPackageFloatingVersionsEnabled> should be defined in the Directory.Packages.props"/*..*/).WithSpan(04, 04, 04, 88),
            Issue.WRN("Proj0808", "The <CentralPackageTransitivePinningEnabled> should be defined in the Directory.Packages.props"/*.*/).WithSpan(05, 04, 05, 90),
            Issue.WRN("Proj0808", "The <CentralPackageVersionOverrideEnabled> should be defined in the Directory.Packages.props"/*...*/).WithSpan(06, 04, 06, 86),
            Issue.WRN("Proj0808", "The <GlobalPackageReference> should be defined in the Directory.Packages.props"/*.................*/).WithSpan(10, 04, 10, 84),
            Issue.WRN("Proj0808", "The <PackageVersion> should be defined in the Directory.Packages.props"/*.........................*/).WithSpan(11, 04, 11, 55));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    [TestCase("UseCPM.cs")]
    public void Projects_without_issues(string project) => new DefineCpmRelatedConfigInDirectoryPackages()
        .ForProject(project)
        .HasNoIssues();
}
