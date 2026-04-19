namespace Specs.Rules.Global_package_references_only_work_with_CPM;

public class Reports
{
    [Test]
    public void GlobalPackageReference_without_CPM() => new GlobalPackageReferencesOnlyWorkWithCpm()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>
        
          <ItemGroup>
            <GlobalPackageReference Include="NuGet.Versioning" Version="7.3.1" />
          </ItemGroup>
        
        </Project>
        """)
        .HasIssue(
            Issue.WRN("Proj0813", "Global package reference 'NuGet.Versioning' is ignored as CMP is disabled").WithSpan(7, 4, 7, 73));
}

public class Guards
{
    [Test]
    public void GlobalPackageReference_with_CPM() => new GlobalPackageReferencesOnlyWorkWithCpm()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
          </PropertyGroup>
        
          <!-- Hack to ensure that things build -->
          <ItemGroup>
            <PackageReference Include="NuGet.Versioning" Version="7.3.1" />
          </ItemGroup>

          <ItemGroup>
            <GlobalPackageReference Include="NuGet.Versioning" Version="7.3.1" />
          </ItemGroup>
        
        </Project>
        """)
        .HasNoIssues();
}
