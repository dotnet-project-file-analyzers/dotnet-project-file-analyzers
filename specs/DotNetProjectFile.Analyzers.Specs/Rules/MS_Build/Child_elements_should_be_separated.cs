namespace Rules.MS_Build.Child_elements_should_be_separated;

public class Reports
{
    [Test]
    public void violations() => new ChildElementsShouldBeSeparated()
        .ForInlineCsproj("""
        <Project Sdk="Microsoft.NET.Sdk">
        
          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
            <EnablePackageValidation>true</EnablePackageValidation>
          </PropertyGroup>
          <PropertyGroup>
            <ImplicitUsings>false</ImplicitUsings>
          </PropertyGroup>
        
          <!-- Comments should be ingored -->
          <ItemGroup>
            <PackageReference Include="DotNetProjectFile.Analyzers" Version="1.8.0" PrivateAssets="all" />
          </ItemGroup>


         <ItemGroup>
            <PackageReference Update="DotNetProjectFile.Analyzers" Version="1.10.0" />
          </ItemGroup>
        
        </Project>
        """)
        .HasIssues(
            Issue.WRN("Proj1703", "Insert empty line").WithSpan(05, 0, 06, 0),
            Issue.WRN("Proj1703", "Remove empty line").WithSpan(13, 0, 14, 0));
}

public class Guards
{
    [Test]
    public void XML_declaration() => new ChildElementsShouldBeSeparated()
        .ForInlineCsproj("""
        <?xml version="1.0" encoding="UTF-8"?>
        <Project Sdk="Microsoft.NET.Sdk">

          <PropertyGroup>
            <TargetFramework>net10.0</TargetFramework>
          </PropertyGroup>

        </Project>
        """)
        .HasNoIssues();
}
