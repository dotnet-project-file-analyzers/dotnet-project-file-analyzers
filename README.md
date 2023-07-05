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
* [**Proj0003** Define usings explicit](rules/Proj0003.md)
* [**Proj1001** Use analyzers for packages](rules/Proj1001.md)
