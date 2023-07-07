# .NET project file analyzers
Contains [Roslyn](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/)
(static code) [diagnostic analyzers](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticanalyzer)
that report issues on .NET project files.

## Additional files
To fully benefit from these analyzers is is recommended to add the project file
(and imported projects/props) as additional files.

In the project file:

``` XML
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <AdditionalFiles Include="*.??proj" Visible="false" />
  </ItemGroup>

</Project>
```

In a props file:

``` XML
<?xml version="1.0" encoding="utf-8"?>
<Project>

  <ItemGroup>
    <AdditionalFiles Include="../props/{file_name}" Link="Properties/{file_name}" />
  </ItemGroup>

</Project>
```

## Rules
* [**Proj0001** .NET project file could not be located](rules/Proj0001.md)
* [**Proj0002** Upgrade legacy .NET project files](rules/Proj0002.md)
* [**Proj0003** Define usings explicit](rules/Proj0003.md)
* [**Proj0004** Run NuGet security audits automatically](rules/Proj0004.md)
* [**Proj0005** Define package reference assets as attributes](rules/Proj0005.md)
* [**Proj0006** Add additional files to improve static code analysis](rules/Proj0006.md)
* [**Proj0007** Remove empty nodes](rules/Proj0007.md)
* [**Proj1000** Use the .NET project file analyzers](rules/Proj1000.md)
* [**Proj1001** Use analyzers for packages](rules/Proj1001.md)

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
