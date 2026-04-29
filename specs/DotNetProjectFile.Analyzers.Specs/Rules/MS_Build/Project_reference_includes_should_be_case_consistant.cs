namespace Rules.MS_Build.Project_reference_includes_should_be_case_consistant;

#if Is_Windows
public class Reports
{
    [Test]
    public void case_mismatch() => new ProjectReferenceChecker()
        .ForProject("ProjectReferenceDifferentCasing.cs")
        .HasIssue(Issue
            .WRN("Proj0051", "The casing of './foo.csproj' of differs from the file 'FOO.csproj' on disk")
            .WithSpan(07, 04, 07, 47));

    [Test]
    public void msbuild_property_case_mismatch() => new ProjectReferenceChecker()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup>
                <ProjectReference Include="$(MSBuildThisFileDirectory)SIBLING.csproj" />
              </ItemGroup>

            </Project>
            """)
        .WithFile("Sibling.csproj", """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
            </Project>
            """)
        .HasIssue(Issue
            .WRN("Proj0051", "The casing of '$(MSBuildThisFileDirectory)SIBLING.csproj' of differs from the file 'Sibling.csproj' on disk"));
}
#endif

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void Projects_with_analyzers(string project) => new ProjectReferenceChecker()
        .ForProject(project)
        .HasNoIssues();
}
