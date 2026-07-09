namespace Diagnostics.NoWarn_specs;

public class Suppresses
{
    [Test]
    public void NoWarn_Attribute() => new RemoveFolderNodes().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <Folder Include="SomeFolder/" NoWarn="Proj0008" />
          </ItemGroup>

          <ItemGroup>
            <Folder Include="OtherFolder/" />
          </ItemGroup>

        </Project>
        """)
        .HasIssue(Issue.WRN("Proj0008", "Remove folder node 'OtherFolder'" /*.*/).WithSpan(11, 4, 11, 37));

    [Test]
    public void NoWarn_Node() => new RemoveFolderNodes().ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

          <ItemGroup>
            <Folder Include="SomeFolder/">
              <NoWarn>Proj0008</NoWarn>
            </Folder>
          </ItemGroup>

          <ItemGroup>
            <Folder Include="OtherFolder/" />
          </ItemGroup>

        </Project>
        """)
        .HasIssue(Issue.WRN("Proj0008", "Remove folder node 'OtherFolder'" /*.*/).WithSpan(13, 4, 13, 37));
}
