namespace Rules.MS_Build.Project_reference_include_should_exist;

public class Reports
{
    [Test]
    public void empty_includes() => new ProjectReferenceChecker()
        .ForProject("ProjectReferenceMissingInclude.cs")
        .HasIssue(Issue
            .WRN("Proj0033", "The Include './foo.csproj' of <ProjectReference> does not exist")
            .WithSpan(07, 04, 07, 47));

    [Test]
    public void msbuild_property_with_missing_file() => new ProjectReferenceChecker()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup>
                <ProjectReference Include="$(MSBuildThisFileDirectory)Missing.csproj" />
              </ItemGroup>

            </Project>
            """)
        .HasIssue(Issue
            .WRN("Proj0033", "The Include '$(MSBuildThisFileDirectory)Missing.csproj' of <ProjectReference> does not exist"));
}

public class Guards
{
    [Test]
    public void Projects_with_analyzers() => new ProjectReferenceChecker()
        .ForProject("CompliantCSharp.cs")
        .HasNoIssues();

    [Test]
    public void Project_reference_with_msbuild_property() => new ProjectReferenceChecker()
        .ForProject("ProjectReferenceWithMsBuildProperty.cs")
        .HasNoIssues();
}
