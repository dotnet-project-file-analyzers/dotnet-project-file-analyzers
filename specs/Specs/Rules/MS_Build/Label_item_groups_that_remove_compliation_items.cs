namespace Rules.MS_Build.Label_item_groups_that_remove_compliation_items;

public class Reports
{
    [Test]
    public void unlabled_item_groups_with_compile_remove() => new LabelItemGroupsThatRemoveCompilationItems()
       .ForInlineCsproj(""""
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <Compile Remove="Program.cs" />
  </ItemGroup>

  <ItemGroup Label="">
    <Compile Remove="Program.cs" />
  </ItemGroup>

  <ItemGroup Label="With description">
    <Compile Remove="Program.cs" />
  </ItemGroup>

  <ItemGroup>
    <Compile Include="Other.cs" />
  </ItemGroup>

</Project>
"""")
       .HasIssues(
            Issue.WRN("Proj0047", "Add a label to this group as it removes items from compilation").WithSpan(05, 02, 07, 14),
            Issue.WRN("Proj0047", "Add a label to this group as it removes items from compilation").WithSpan(09, 02, 11, 14));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => new LabelItemGroupsThatRemoveCompilationItems()
        .ForProject(project)
        .HasNoIssues();
}
