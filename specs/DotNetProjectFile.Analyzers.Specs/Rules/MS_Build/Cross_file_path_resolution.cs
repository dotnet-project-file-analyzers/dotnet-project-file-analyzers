namespace Rules.MS_Build.Cross_file_path_resolution;

// Integration tests for the dual-anchor resolution rule (issue 546):
// MSBuildThisFile family properties resolve against the file the reference lives in;
// MSBuildProject family properties and bare relative paths resolve against the entry-point project file.

public class Build_action_include_should_exist
{
    [Test]
    public void Bare_path_in_props_finds_file_in_csproj_directory_not_props_directory()
        => new BuildActionIncludeShouldExist()
            .ForInlineProject("Inner/inline.csproj", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                  </PropertyGroup>
                </Project>
                """)
            .WithFile("Directory.Build.props", """
                <Project>
                  <ItemGroup>
                    <None Include="NextToCsproj.txt" />
                  </ItemGroup>
                </Project>
                """)
            .WithFile("Inner/NextToCsproj.txt", string.Empty)
            .HasNoIssues();

    [Test]
    public void Bare_path_in_props_does_not_resolve_against_props_directory()
        => new BuildActionIncludeShouldExist()
            .ForInlineProject("Inner/inline.csproj", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                  </PropertyGroup>
                </Project>
                """)
            .WithFile("Directory.Build.props", """
                <Project>
                  <ItemGroup>
                    <None Include="OnlyAtPropsDir.txt" />
                  </ItemGroup>
                </Project>
                """)
            .WithFile("OnlyAtPropsDir.txt", string.Empty)
            .HasIssue(Issue
                .WRN("Proj0022", "The Include 'OnlyAtPropsDir.txt' of <None> does not exist"));

    [Test]
    public void MSBuildThisFileDirectory_inside_props_resolves_to_props_directory()
        => new BuildActionIncludeShouldExist()
            .ForInlineProject("Inner/inline.csproj", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                  </PropertyGroup>
                </Project>
                """)
            .WithFile("Directory.Build.props", """
                <Project>
                  <ItemGroup>
                    <None Include="$(MSBuildThisFileDirectory)AtPropsDir.txt" />
                  </ItemGroup>
                </Project>
                """)
            .WithFile("AtPropsDir.txt", string.Empty)
            .HasNoIssues();

    [Test]
    public void MSBuildProjectDirectory_inside_props_resolves_to_csproj_directory()
        => new BuildActionIncludeShouldExist()
            .ForInlineProject("Inner/inline.csproj", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                  </PropertyGroup>
                </Project>
                """)
            .WithFile("Directory.Build.props", """
                <Project>
                  <ItemGroup>
                    <None Include="$(MSBuildProjectDirectory)/AtCsprojDir.txt" />
                  </ItemGroup>
                </Project>
                """)
            .WithFile("Inner/AtCsprojDir.txt", string.Empty)
            .HasNoIssues();
}

public class Imported_targets_file
{
    [Test]
    public void MSBuildThisFileDirectory_inside_Directory_Build_targets_resolves_to_targets_directory()
        => new BuildActionIncludeShouldExist()
            .ForInlineProject("Inner/inline.csproj", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                  </PropertyGroup>
                </Project>
                """)
            .WithFile("Directory.Build.targets", """
                <Project>
                  <ItemGroup>
                    <None Include="$(MSBuildThisFileDirectory)NextToTargets.txt" />
                  </ItemGroup>
                </Project>
                """)
            .WithFile("NextToTargets.txt", string.Empty)
            .HasNoIssues();
}
