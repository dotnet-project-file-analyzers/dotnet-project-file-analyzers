namespace Rules.MS_Build.Package_references_should_be_stable;

public class Reports
{
    [Test]
    public void unstable_versions() => new PackageReferencesShouldBeStable()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <PackageReference Include="Qowaiv" Version="8.0.0" />
            <PackageReference Include="System.IO.Hashing" Version="9.0.0-preview.7.24405.7" />
          </ItemGroup>

        </Project>
        """)
        .HasIssue(Issue
            .WRN("Proj1101", "Use a stable version of 'System.IO.Hashing', instead of '9.0.0-preview.7.24405.7'")
            .WithSpan(08, 04, 08, 86));

    [Test]
    public void unstable_versions_via_CPM() => new PackageReferencesShouldBeStable()
        .ForProject("UnstableVersionsCPM.cs")
        .HasIssues(
            Issue.WRN("Proj1101", "Use a stable version of 'StyleCop.Analyzers', instead of '*-*'")
                .WithSpan(08, 04, 08, 65).WithPath("Directory.Packages.props"),
            Issue.WRN("Proj1101", "Use a stable version of 'System.IO.Hashing', instead of '9.0.0-preview.7.24405.7'")
                .WithSpan(09, 04, 09, 84).WithPath("Directory.Packages.props"),
            Issue.WRN("Proj1101", "Use a stable version of 'System.IO.Hashing', instead of '9.0.0-rc.2.24473.5'")
                .WithSpan(09, 04, 09, 88).WithPath("UnstableVersionsCPM.csproj"),
            Issue.WRN("Proj1101", "Use a stable version of 'Warpstone', instead of '2.0.0-preview2'")
                .WithSpan(10, 04, 10, 67).WithPath("Directory.Packages.props"));


    [Test]
    public void unstable_global_package_references() => new PackageReferencesShouldBeStable()
        .ForInlineCsproj(
        """
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <GlobalPackageReference Include="StyleCop.Analyzers" Version="1.2.0-beta.556" />
          </ItemGroup>
        
        </Project>
        """)
        .HasIssue(Issue
            .WRN("Proj1101", "Use a stable version of 'StyleCop.Analyzers', instead of '1.2.0-beta.556'")
            .WithSpan(07, 04, 07, 84));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void project_files_as_additional(string project)
         => new AddAdditionalFile()
        .ForProject(project)
        .HasNoIssues();
}
