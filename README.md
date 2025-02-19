[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/Corniel/dotnet-project-files-analyzers/blob/main/LICENSE.md)
[![Code of Conduct](https://img.shields.io/badge/%E2%9D%A4-code%20of%20conduct-blue.svg?style=flat)](https://github.com/Corniel/dotnet-project-files-analyzers/blob/main/CODE_OF_CONDUCT.md)

![.NET project file analyzers logo](design/logo_128x128.png)
# .NET project file analyzers
Contains 110+ [Roslyn](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/)
(static code) [diagnostic analyzers](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticanalyzer)
that report issues on .NET project files.

## Documentation
The full documentation can be found here: [dotnet-project-file-analyzers.github.io](https://dotnet-project-file-analyzers.github.io/)

## Installation
| Packages                                                                                         | NuGet                                                                                                                                                                                                                                                                    |
|--------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|[DotNetProjectFile.Analyzers](https://www.nuget.org/packages/DotNetProjectFile.Analyzers/)        | [![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers/)                     |
|[DotNetProjectFile.Analyzers.Sdk](https://www.nuget.org/packages/DotNetProjectFile.Analyzers.Sdk/)| [![DotNetProjectFile.Analyzers.Sdk](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers.Sdk)![DotNetProjectFile.Analyzers.Sdk](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers.Sdk)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers.Sdk/) |

### Analyzers
To use the analyzers, you must include the analyzer NuGet package in your project file:
``` XML
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <PackageReference Include="DotNetProjectFile.Analyzers" Version="*" PrivateAssets="all" />
  </ItemGroup>

</Project>
```

or via PowerShell:

``` PS
Install-Package DotNetProjectFile.Analyzers
```
### SDK
To use the SDK, follow the instructions [here](https://dotnet-project-file-analyzers.github.io/general/sdk.html).

## Additional files
To fully benefit from these analyzers it is recommended to add the project file
(and RESX, and imported projects/props) as additional files.

To add a project file:

``` XML
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <AdditionalFiles Include="*.??proj" Visible="false" />
	<AdditionalFiles Include="*.resx" />
  </ItemGroup>

</Project>
```

To add a props file:

``` XML
<Project>

  <ItemGroup>
    <AdditionalFiles Include="../props/{file_name}" Link="Properties/{file_name}" />
	<AdditionalFiles Include="*.resx" />
  </ItemGroup>

</Project>
```

## ANTLR
For parsing non-XML based syntaxes we depend on [ANTLR](https://www.antlr.org)
by Terence Parr. We included the sources of [Antlr4.Runtime](https://github.com/antlr/antlr4)
to prevent dependency issues when executing the analyzers.

## Community
* [LinkedIn](https://www.linkedin.com/company/dotnet-project-file-analyzers)

## Reference an analyzer from a project
For debugging/development purposes, it can be useful to reference the analyzer
project directly. As mentioned [here](https://www.meziantou.net/referencing-an-analyzer-from-a-project.htm),
this can be achieved by adding the following to your project file:

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
