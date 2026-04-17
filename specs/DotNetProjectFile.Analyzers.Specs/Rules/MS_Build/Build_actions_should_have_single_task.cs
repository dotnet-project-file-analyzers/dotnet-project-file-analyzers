namespace Rules.MS_Build.Build_actions_should_have_single_task;

public class Reports
{
    [Test]
    public void multple_tasks() => new BuildActionsShouldHaveSingleTask()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <Nullable>enable</Nullable>
          </PropertyGroup>

          <ItemGroup>
            <Compile Include="../common/Code.cs" />
          </ItemGroup>

          <ItemGroup>
            <Content Update="README.md;../common/Code.cs" />
          </ItemGroup>

          <ItemGroup>
            <None Include="../common/Code.cs;README.md" />
          </ItemGroup>

          <ItemGroup>
            <AdditionalFiles Include="*.csproj;*.vbproj" Visible="false" />
          </ItemGroup>

          <ItemGroup>
            <GlobalAnalyzerConfigFiles
              Include="$([MSBuild]::GetPathOfFileAbove('.globalconfig', '$(MSBuildThisFileDirectory)'))"
              Exclude="$(GlobalAnalyzerConfigFiles)" />
          </ItemGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0021", "The <Content> defines multiple tasks" /*...*/).WithSpan(12, 04, 12, 52),
            Issue.WRN("Proj0021", "The <None> defines multiple tasks" /*......*/).WithSpan(16, 04, 16, 50),
            Issue.WRN("Proj0021", "The <AdditionalFiles> defines multiple tasks").WithSpan(20, 04, 20, 67)
        );
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    public void compliant_projects(string project) => new BuildActionsShouldHaveSingleTask()
        .ForProject(project)
        .HasNoIssues();
}
