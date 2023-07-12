[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/Corniel/dotnet-project-files-analyzers/blob/main/LICENSE.md)
[![Code of Conduct](https://img.shields.io/badge/%E2%9D%A4-code%20of%20conduct-blue.svg?style=flat)](https://github.com/Corniel/dotnet-project-files-analyzers/blob/main/CODE_OF_CONDUCT.md)

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
* [**Proj1000** Use the .NET project file analyzers](rules/Proj1000.md)
* [**Proj1001** Use analyzers for packages](rules/Proj1001.md)
* [**Proj1003** Use Sonar analyzers](rules/Proj1003.md)

## Resource file rules
* [**Proj2000** Embed valid resource files](rules/Proj2000.md)
* [**Proj2001** Define data in a resource file](rules/Proj2001.md)
* [**Proj2002** Sort resource file values alphabetically](rules/Proj2002.md)
* [**Proj2003** Add invariant fallback resources](rules/Proj2003.md)

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

More info can be found here: https://www.meziantou.net/referencing-an-analyzer-from-a-project.htm
