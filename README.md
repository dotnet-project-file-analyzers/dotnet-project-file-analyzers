| Package | NuGet |
|---------|-------|
| [DotNetProjectFile.Analyzers](https://www.nuget.org/packages/DotNetProjectFile.Analyzers/) | [![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers?style=flat-square&label=version)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers?style=flat-square)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers/) |

![.NET project file analyzers logo](design/logo_128x128.png)
# .NET project file analyzers
Contains [Roslyn](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/)
(static code) [diagnostic analyzers](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticanalyzer)
that report issues on .NET project files.

## Installation
To use the analyzers, you must include the analyzer package in your project file:
``` XML
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <PackageReference Include="DotNetProjectFile.Analyzers" Version="1.*" PrivateAssets="all" IncludeAssets="runtime; build; native; contentfiles; analyzers; buildtransitive" />
  </ItemGroup>

</Project>
```

## GitHub repository
The source code can be found at GitHub: [github.com/dotnet-project-file-analyzers](https://github.com/dotnet-project-file-analyzers).

## MS Build project file rules
* [**Proj0001** MS Build project file could not be located](rules/Proj0001.md)
* [**Proj0002** Upgrade legacy MS Build project files](rules/Proj0002.md)
* [**Proj0003** Define usings explicit](rules/Proj0003.md)
* [**Proj0004** Run NuGet security audits automatically](rules/Proj0004.md)
* [**Proj0005** Define package reference assets as attributes](rules/Proj0005.md)
* [**Proj0006** Add additional files to improve static code analysis](rules/Proj0006.md)
* [**Proj0007** Remove empty nodes](rules/Proj0007.md)
* [**Proj0008** Remove folder nodes](rules/Proj0008.md)
* [**Proj0009** Use the TragetFramework node for a single target framework](rules/Proj0009.md)
* [**Proj0010** Define OutputType explicitly](rules/Proj0010.md)
* [**Proj0011** Define properties once](rules/Proj0011.md)
* [**Proj0012** Reassign properties with different value](rules/Proj0012.md)
* [**Proj0013** Include package references only once](rules/Proj0013.md)
* [**Proj0014** Include project references only once](rules/Proj0014.md)
* [**Proj0015** Order package references alphabetically](rules/Proj0015.md)
* [**Proj0016** Order project references alphabetically](rules/Proj0016.md)
* [**Proj0017** Can't create alias for static using directive](rules/Proj0017.md)
* [**Proj0018** Order using directives by type](rules/Proj0018.md)
* [**Proj0019** Order using directives alphabetically](rules/Proj0019.md)
* [**Proj0200** Define IsPackable explicitly](rules/Proj0200.md)
* [**Proj0201** Define the project version explicitly](rules/Proj0201.md)
* [**Proj0202** Define the project description explicitly](rules/Proj0202.md)
* [**Proj0203** Define the project authors explicitly](rules/Proj0203.md)
* [**Proj0204** Define the project tags explicitly](rules/Proj0204.md)
* [**Proj0205** Define the project repository URL explicitly](rules/Proj0205.md)
* [**Proj0206** Define the project URL explicitly](rules/Proj0206.md)
* [**Proj0207** Define the project copyright explicitly](rules/Proj0207.md)
* [**Proj0208** Define the project release notes explicitly](rules/Proj0208.md)
* [**Proj0209** Define the project readme file explicitly](rules/Proj0209.md)
* [**Proj0210** Define the project license explicitly](rules/Proj0210.md)
* [**Proj0211** Avoid using deprecated license definition](rules/Proj0211.md)
* [**Proj0212** Define the project icon file explicitly](rules/Proj0212.md)
* [**Proj0213** Define the project icon URL explicitly](rules/Proj0213.md)
* [**Proj0214** Define the NuGet project ID explicitly](rules/Proj0214.md)
* [**Proj0400** Define the project publishability explicitly](rules/Proj0400.md)
* [**Proj1000** Use the .NET project file analyzers](rules/Proj1000.md)
* [**Proj1001** Use analyzers for packages](rules/Proj1001.md)
* [**Proj1003** Use Sonar analyzers](rules/Proj1003.md)

## Resource file rules
* [**Proj2000** Embed valid resource files](rules/Proj2000.md)
* [**Proj2001** Define data in a resource file](rules/Proj2001.md)
* [**Proj2002** Sort resource file values alphabetically](rules/Proj2002.md)
* [**Proj2003** Add invariant fallback resources](rules/Proj2003.md)
* [**Proj2004** Add invariant fallback values](rules/Proj2004.md)

## Additional files
To fully benefit from these analyzers it is recommended to add the project file
(and imported projects/props) as additional files.

To add a project file:

``` XML
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <AdditionalFiles Include="*.??proj" Visible="false" />
  </ItemGroup>

</Project>
```

To add a props file:

``` XML
<?xml version="1.0" encoding="utf-8"?>
<Project>

  <ItemGroup>
    <AdditionalFiles Include="../props/{file_name}" Link="Properties/{file_name}" />
  </ItemGroup>

</Project>
```

## Reference an analyzer from a project
For debugging/development purposes, it can be useful to reference the analyzer
project directly. Within this solution, that would look like:

``` XML
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup Label="Analyzer">
    <ProjectReference
      Include="../../src/DotNetProjectFile.Analyzers/DotNetProjectFile.Analyzers.csproj"
      PrivateAssets="all"
      ReferenceOutputAssembly="false"
      OutputItemType="Analyzer"
      SetTargetFramework="TargetFramework=netstandard2.0" />
  </ItemGroup>

</Project>
```

More info about referencing project files can be found at [meziantou.net](https://www.meziantou.net/referencing-an-analyzer-from-a-project.htm).
