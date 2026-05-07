namespace Rules.MS_Build.MsBuild_property_should_be_resolvable;

public class Reports
{
    [Test]
    public void unresolved_property_in_project_reference_include() => new MsBuildPropertyShouldBeResolvable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
              <ItemGroup>
                <ProjectReference Include="$(SomeUnknownProperty)/Foo.csproj" />
              </ItemGroup>
            </Project>
            """)
        .HasIssue(Issue.INF("Proj0052",
            "The MSBuild property '$(SomeUnknownProperty)' referenced in '$(SomeUnknownProperty)/Foo.csproj' could not be resolved"));

    [Test]
    public void unresolved_property_in_build_action_include() => new MsBuildPropertyShouldBeResolvable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
              <ItemGroup>
                <None Include="$(MyCustomProp)/Foo.txt" />
              </ItemGroup>
            </Project>
            """)
        .HasIssue(Issue.INF("Proj0052",
            "The MSBuild property '$(MyCustomProp)' referenced in '$(MyCustomProp)/Foo.txt' could not be resolved"));

    [Test]
    public void multiple_distinct_unresolved_properties_each_fire() => new MsBuildPropertyShouldBeResolvable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
              <ItemGroup>
                <ProjectReference Include="$(FirstUnknown)/$(SecondUnknown)/Foo.csproj" />
              </ItemGroup>
            </Project>
            """)
        .HasIssues(
            Issue.INF("Proj0052",
                "The MSBuild property '$(FirstUnknown)' referenced in '$(FirstUnknown)/$(SecondUnknown)/Foo.csproj' could not be resolved"),
            Issue.INF("Proj0052",
                "The MSBuild property '$(SecondUnknown)' referenced in '$(FirstUnknown)/$(SecondUnknown)/Foo.csproj' could not be resolved"));
}

public class Guards
{
    [Test]
    public void reserved_property_does_not_fire() => new MsBuildPropertyShouldBeResolvable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
              <ItemGroup>
                <ProjectReference Include="$(MSBuildThisFileDirectory)Foo.csproj" />
              </ItemGroup>
            </Project>
            """)
        .HasNoIssues();

    [Test]
    public void user_defined_property_in_unconditional_property_group_does_not_fire() => new MsBuildPropertyShouldBeResolvable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <MyCustomDir>some/path</MyCustomDir>
              </PropertyGroup>
              <ItemGroup>
                <ProjectReference Include="$(MyCustomDir)/Foo.csproj" />
              </ItemGroup>
            </Project>
            """)
        .HasNoIssues();

    [Test]
    public void user_defined_property_only_in_conditional_group_is_treated_as_unresolved() => new MsBuildPropertyShouldBeResolvable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
              <PropertyGroup Condition="'a' == 'a'">
                <MyConditionalDir>some/path</MyConditionalDir>
              </PropertyGroup>
              <ItemGroup>
                <ProjectReference Include="$(MyConditionalDir)/Foo.csproj" />
              </ItemGroup>
            </Project>
            """)
        .HasIssue(Issue.INF("Proj0052",
            "The MSBuild property '$(MyConditionalDir)' referenced in '$(MyConditionalDir)/Foo.csproj' could not be resolved"));

    [Test]
    public void user_defined_property_in_conditional_child_of_unconditional_group_is_treated_as_unresolved() => new MsBuildPropertyShouldBeResolvable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <MyConditionalProp Condition="'a' == 'a'">some/path</MyConditionalProp>
              </PropertyGroup>
              <ItemGroup>
                <ProjectReference Include="$(MyConditionalProp)/Foo.csproj" />
              </ItemGroup>
            </Project>
            """)
        .HasIssue(Issue.INF("Proj0052",
            "The MSBuild property '$(MyConditionalProp)' referenced in '$(MyConditionalProp)/Foo.csproj' could not be resolved"));

    [Test]
    public void include_without_property_reference_does_not_fire() => new MsBuildPropertyShouldBeResolvable()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
              <ItemGroup>
                <None Include="just-a-literal.txt" />
              </ItemGroup>
            </Project>
            """)
        .HasNoIssues();
}
