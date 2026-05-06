namespace Rules.MS_Build.Remove_redundant_references_for_globally_referenced;

public class Reports
{
    [Test]
    public void on_redundant_included() => new RemoveRedundantReferencesForGloballyReferenced()
            .ForProject("RemoveRedundantReferencesForGloballyReferenced.cs")
            .HasIssue(
                Issue.WRN("Proj0812", "Remove redundant reference 'DotNetProjectFile.Analyzers'").WithSpan(07, 04, 07, 62));
}

public class Guards
{
    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void project_files_as_additional(string project) => new RemoveRedundantReferencesForGloballyReferenced()
        .ForProject(project)
        .HasNoIssues();

    [Test]
    public void Reference_inside_false_condition_item_group_does_not_fire() => new RemoveRedundantReferencesForGloballyReferenced()
        .ForInlineCsproj("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
              </PropertyGroup>
              <ItemGroup>
                <GlobalPackageReference Include="Qowaiv" Version="7.0.0" />
              </ItemGroup>
              <ItemGroup Condition="'a' == 'b'">
                <PackageReference Include="Qowaiv" Version="7.0.0" />
              </ItemGroup>
            </Project>
            """)
        .HasNoIssues();
}
