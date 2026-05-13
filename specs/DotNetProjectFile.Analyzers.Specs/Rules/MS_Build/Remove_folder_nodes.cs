namespace Rules.MS_Build.Remove_folder_nodes;

public class Reports
{
    [Test]
    public void folder_nodes() => new RemoveFolderNodes().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <Nullable>enable</Nullable>
          </PropertyGroup>

          <ItemGroup>
            <Folder Include="SomeFolder\" />
          </ItemGroup>

          <ItemGroup>
            <Folder Include="OtherFolder/" />
          </ItemGroup>

        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj0008", "Remove folder node 'SomeFolder'" /*..*/).WithSpan(08, 4, 08, 36),
            Issue.WRN("Proj0008", "Remove folder node 'OtherFolder'" /*.*/).WithSpan(12, 4, 12, 37));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_with_analyzers(string project) => new RemoveFolderNodes()
        .ForProject(project)
        .HasNoIssues();
}
