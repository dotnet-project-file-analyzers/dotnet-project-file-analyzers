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

    [Test]
    public void condition_evaluating_to_true_still_reports() => new ProjectReferenceChecker()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup Condition="'a' == 'a'">
                <ProjectReference Include="Missing.csproj" />
              </ItemGroup>

            </Project>
            """)
        .HasIssue(Issue
            .WRN("Proj0033", "The Include 'Missing.csproj' of <ProjectReference> does not exist"));

    [Test]
    public void unevaluable_condition_is_treated_conservatively_as_included() => new ProjectReferenceChecker()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup Condition="'$(SomeUnknownProperty)' == 'foo'">
                <ProjectReference Include="Missing.csproj" />
              </ItemGroup>

            </Project>
            """)
        .HasIssue(Issue
            .WRN("Proj0033", "The Include 'Missing.csproj' of <ProjectReference> does not exist"));
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

    [Test]
    public void Conditional_item_group_evaluating_to_false_is_skipped() => new ProjectReferenceChecker()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup Condition="'a' == 'b'">
                <ProjectReference Include="Missing.csproj" />
              </ItemGroup>

            </Project>
            """)
        .HasNoIssues();

    [Test]
    public void Conditional_item_referencing_msbuild_property_is_skipped_when_property_excludes_it() => new ProjectReferenceChecker()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">

              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>

              <ItemGroup Condition="'$(MSBuildProjectExtension)' == '.vbproj'">
                <ProjectReference Include="Missing.csproj" />
              </ItemGroup>

            </Project>
            """)
        .HasNoIssues();
}
