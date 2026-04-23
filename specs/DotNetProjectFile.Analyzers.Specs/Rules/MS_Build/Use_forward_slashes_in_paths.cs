namespace Rules.Use_forward_slashes_in_paths;

public class Reports
{
    [Test]
    public void backward_slashes() => new UseForwardSlashesInPaths().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <DockerfileContext>..\..</DockerfileContext>
          </PropertyGroup>

          <ItemGroup>
            <Compile Include="..\common\Code.cs" Link="Include\Code.cs" />
          </ItemGroup>

          <ItemGroup>
            <Folder Include="SomeFolder\Child" />
          </ItemGroup>

          <ItemGroup>
            <None Remove="Files\*" />
          </ItemGroup>

        </Project>
        """)
       .HasIssues(
           Issue.WRN("Proj0023", "<DockerfileContext> contains backward slashes" /*.*/).WithSpan(04, 4, 04, 48),
           Issue.WRN("Proj0023", "<Compile Include> contains backward slashes" /*...*/).WithSpan(08, 4, 08, 66),
           Issue.WRN("Proj0023", "<Compile Link> contains backward slashes" /*......*/).WithSpan(08, 4, 08, 66),
           Issue.WRN("Proj0023", "<Folder Include> contains backward slashes" /*....*/).WithSpan(12, 4, 12, 41),
           Issue.WRN("Proj0023", "<None Remove> contains backward slashes" /*.......*/).WithSpan(16, 4, 16, 29));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new UseForwardSlashesInPaths()
        .ForProject(project)
        .HasNoIssues();
}
