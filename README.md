[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://github.com/Corniel/dotnet-project-files-analyzers/blob/main/LICENSE.md)
[![Code of Conduct](https://img.shields.io/badge/%E2%9D%A4-code%20of%20conduct-blue.svg?style=flat)](https://github.com/Corniel/dotnet-project-files-analyzers/blob/main/CODE_OF_CONDUCT.md)

![.NET project file analyzers logo](design/logo_128x128.png)
# .NET project file analyzers
Contains [Roslyn](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/)
(static code) [diagnostic analyzers](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticanalyzer)
that report issues on .NET project files.

## Installation
| Package                                                                                  | NuGet                                                                                                                                                                                                                                                |
|:----------------------------------------------------------------------------------------:|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|[DotNetProjectFile.Analyzers](https://www.nuget.org/packages/DotNetProjectFile.Analyzers/)| [![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers/) |

To use the analyzers, you must include the analyzer NuGet package in your project file:
``` XML
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <PackageReference Include="DotNetProjectFile.Analyzers" Version="*" PrivateAssets="all" IncludeAssets="runtime; build; native; contentfiles; analyzers; buildtransitive" />
  </ItemGroup>

</Project>
```

or via PowerShell:

``` PS
Install-Package DotNetProjectFile.Analyzers
```

## Rules
This package contains rules about the MS Build project files (including
imported props), and RESX files. The complete overview can be found at
[dotnet-project-file-analyzers.github.io](https://dotnet-project-file-analyzers.github.io/).

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

## Adjusting severity
As of the time of writing, it is [unfortunately not possible](https://github.com/dotnet/roslyn/issues/37876) to change the severity of rules using an `.editorconfig` file.
It is however possible to adjust the severity using a [Global AnalyzerConfig](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files#global-analyzerconfig)
(`.globalconfig`) file located in the same directory as the project file or in one of its (grand)parent directories.
The following `.globalconfig` file will disable rule `Proj0010` and raise `Proj0011` to error level:

``` INI
is_global = true

dotnet_diagnostic.Proj0010.severity = none  # Define the <OutputType> node explicitly.
dotnet_diagnostic.Proj0011.severity = error # Property <{0}> has been already defined.
```

Furthermore, it is also possible to disable warnings through the `<NoWarn>` tags inside a `<PropertyGroup>` tag inside your `.csproj` file.
An example of disabling rules `Proj0010` and `Proj0011` through the `.csproj` file:

``` XML
<PropertyGroup>
  <NoWarn>Proj0010;Proj0011</NoWarn>
</PropertyGroup>
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

See also: [www.meziantou.net](https://www.meziantou.net/referencing-an-analyzer-from-a-project.htm)
