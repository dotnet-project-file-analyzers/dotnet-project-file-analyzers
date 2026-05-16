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
            .WRN("Proj0033", "The Include '$(MSBuildThisFileDirectory)Missing.csproj' of <ProjectReference> does not exist (resolved to 'Missing.csproj')"));

    [Test]
    public void multiple_resolvable_properties_in_include_combine_in_resolved_suffix() => new ProjectReferenceChecker()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup>
                <ProjectReference Include="$(MSBuildThisFileDirectory)Sub/$(MSBuildThisFileName)Missing.csproj" />
              </ItemGroup>

            </Project>
            """)
        .HasIssue(Issue
            .WRN("Proj0033", "The Include '$(MSBuildThisFileDirectory)Sub/$(MSBuildThisFileName)Missing.csproj' of <ProjectReference> does not exist (resolved to 'Sub/inlineMissing.csproj')"));

    [Test]
    public void mixed_resolvable_and_unresolved_properties_suppress_the_resolved_suffix() => new ProjectReferenceChecker()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup>
                <ProjectReference Include="$(MSBuildThisFileDirectory)$(UnknownProp)Foo.csproj" />
              </ItemGroup>

            </Project>
            """)
        .HasIssue(Issue
            .WRN("Proj0033", "The Include '$(MSBuildThisFileDirectory)$(UnknownProp)Foo.csproj' of <ProjectReference> does not exist"));
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
