![.NET project file analyzers logo](../design/logo_128x128.png)
# .NET project file analyzers
is a [NuGet](https://www.nuget.org/packages/DotNetProjectFile.Analyzers/) package
containing [Roslyn](https://docs.microsoft.com/dotnet/csharp/roslyn-sdk/)
(static code) [analyzers](https://docs.microsoft.com/dotnet/api/microsoft.codeanalysis.diagnostics.diagnosticanalyzer)
that report issues on .NET project files.

The documentation reflects the current repository state of the features, not the
released ones. Check the [Release Notes](https://www.nuget.org/packages/DotNetProjectFile.Analyzers#releasenotes-body-tab)
to understand if the documented feature you want to use has been released.

## Purpose
We consider all files in a project - so not only those who are compiled - part
of the codebase. We strongly believe that all files should be easy to read,
maintain, or to adjust. Our analyzers help with that. They spot noise, bugs,
inconsistencies, incorrect formatting, and misusage.

All rules come with a clear explanation on why the spotted issue is a bad
practice, and how the code should be adjusted. We hope, that as a result,
developers using our analyzers also learn a thing while working with them.

## Installation
[![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers)

To use the analyzers, you must include the analyzer package in your project file:
``` xml
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <PackageReference Include="DotNetProjectFile.Analyzers" Version="*" PrivateAssets="all" />
  </ItemGroup>

</Project>
```
Or via the command line:
``` bash
dotnet package add DotNetProjectFile.Analyzers
```

## Configuration
How to configure (the severity of) rules is described [here](configuration.md).

## Sdk
.NET Project File Analyzers ships with its own SDK. This allows files shared by
multiple projects to be analyzed. More info can be found [here](sdk.md).

## GitHub repository
The source code can be found at GitHub: [github.com/dotnet-project-file-analyzers](https://github.com/dotnet-project-file-analyzers/dotnet-project-file-analyzers).

## Issues and suggestions
Issues (false positives, false negatives, etc.), and (rule) suggestions can be
reported a the [GibHub repository](https://github.com/dotnet-project-file-analyzers/dotnet-project-file-analyzers/issues).

## MS Build project file rules
* [**Proj0001** MS Build project file could not be located](rules/MSBuild/General/Proj0001.md)
* [**Proj0002** Upgrade legacy MS Build project files](rules/MSBuild/General/Proj0002.md)
* [**Proj0003** Define usings explicit](rules/MSBuild/General/Proj0003.md)
* [**Proj0004** Run NuGet security audits automatically](rules/MSBuild/General/Proj0004.md)
* [**Proj0005** Define package reference assets as attributes](rules/MSBuild/General/Proj0005.md)
* [**Proj0006** Add additional files to improve static code analysis](rules/MSBuild/General/Proj0006.md)
* [**Proj0007** Remove empty nodes](rules/MSBuild/General/Proj0007.md)
* [**Proj0008** Remove folder nodes](rules/MSBuild/General/Proj0008.md)
* [**Proj0009** Use the TragetFramework node for a single target framework](rules/MSBuild/General/Proj0009.md)
* [**Proj0010** Define OutputType explicitly](rules/MSBuild/General/Proj0010.md)
* [**Proj0011** Define properties once](rules/MSBuild/General/Proj0011.md)
* [**Proj0012** Reassign properties with different value](rules/MSBuild/General/Proj0012.md)
* [**Proj0013** Include package references only once](rules/MSBuild/General/Proj0013.md)
* [**Proj0014** Include project references only once](rules/MSBuild/General/Proj0014.md)
* [**Proj0015** Order package references alphabetically](rules/MSBuild/General/Proj0015.md)
* [**Proj0016** Order project references alphabetically](rules/MSBuild/General/Proj0016.md)
* [**Proj0017** Can't create alias for static using directive](rules/MSBuild/General/Proj0017.md)
* [**Proj0018** Order using directives by type](rules/MSBuild/General/Proj0018.md)
* [**Proj0019** Order using directives alphabetically](rules/MSBuild/General/Proj0019.md)
* [**Proj0020** Item group should only contain nodes of a single type](rules/MSBuild/General/Proj0020.md)
* [**Proj0021** Build actions should have a single task](rules/MSBuild/General/Proj0021.md)
* [**Proj0022** Build action includes should exist](rules/MSBuild/General/Proj0022.md)
* [**Proj0023** Use forward slashes in paths](rules/MSBuild/General/Proj0023.md)
* [**Proj0024** Order package versions alphabetically](rules/MSBuild/General/Proj0024.md)
* [**Proj0025** Migrate from rules/MSBuild/Generalet file to .globalconfig file](rules/MSBuild/General/Proj0025.md)
* [**Proj0026** Remove IncludeAssets when redundant](rules/MSBuild/General/Proj0026.md)
* [**Proj0027** Override &lt;TargetFrameworks&gt; with &lt;TargetFrameworks&gt;](rules/MSBuild/General/Proj0027.md)
* [**Proj0028** Define conditions on level 1](rules/MSBuild/General/Proj0028.md)
* [**Proj0029** Use C# specific properties only when applicable](rules/MSBuild/General/Proj0029.md)
* [**Proj0030** Use VB.NET specific properties only when applicable](rules/MSBuild/General/Proj0030.md)
* [**Proj0031** Adopt preferred casing of nodes](rules/MSBuild/General/Proj0031.md)
* [**Proj0032** Migrate away from BinaryFormatter](rules/MSBuild/General/Proj0032.md)
* [**Proj0033** Project reference includes should exist](rules/MSBuild/General/Proj0033.md)
* [**Proj0034** Import statement could not be resolved by the analyzer](rules/MSBuild/General/Proj0034.md)
* [**Proj0035** Remove deprecated RestoreProjectStyle property](rules/MSBuild/General/Proj0035.md)
* [**Proj0036** Remove None when redundant](rules/MSBuild/General/Proj0036.md)
* [**Proj0037** Exclude runtime when all assets are private](rules/MSBuild/General/Proj0037.md)
* [**Proj0038** Fully specify NoWarn rule IDs](rules/MSBuild/General/Proj0038.md)
* [**Proj0039** Treat all warnings as errors is considered a bad practice](rules/MSBuild/General/Proj0039.md)
* [**Proj0040** Run NuGet security audits on transitive dependencies too](rules/MSBuild/General/Proj0040.md)
* [**Proj0041** NuGet security audits should report on moderate issues at minimum](rules/MSBuild/General/Proj0041.md)
* [**Proj0042** Enable &lt;ContinuousIntegrationBuild&gt; when running in CI pipeline](rules/MSBuild/General/Proj0042.md)
* [**Proj0043** Use lock files](rules/MSBuild/General/Proj0043.md)
* [**Proj0044** Enable &lt;RestoreLockedMode&gt; when &lt;ContinuousIntegrationBuild&gt; is enabled](rules/MSBuild/General/Proj0044.md)
* [**Proj0045** Convention-based MSBuild file names should use correct casing](rules/MSBuild/General/Proj0045.md)

### Packaging
* [**Proj0200** Define IsPackable explicitly](rules/MSBuild/Packaging/Proj0200.md)
* [**Proj0201** Define the project version explicitly](rules/MSBuild/Packaging/Proj0201.md)
* [**Proj0202** Define the project description explicitly](rules/MSBuild/Packaging/Proj0202.md)
* [**Proj0203** Define the project authors explicitly](rules/MSBuild/Packaging/Proj0203.md)
* [**Proj0204** Define the project tags explicitly](rules/MSBuild/Packaging/Proj0204.md)
* [**Proj0205** Define the project repository URL explicitly](rules/MSBuild/Packaging/Proj0205.md)
* [**Proj0206** Define the project URL explicitly](rules/MSBuild/Packaging/Proj0206.md)
* [**Proj0207** Define the project copyright explicitly](rules/MSBuild/Packaging/Proj0207.md)
* [**Proj0208** Define the project release notes explicitly](rules/MSBuild/Packaging/Proj0208.md)
* [**Proj0209** Define the project readme file explicitly](rules/MSBuild/Packaging/Proj0209.md)
* [**Proj0210** Define the project license explicitly](rules/MSBuild/Packaging/Proj0210.md)
* [**Proj0211** Avoid using deprecated license definition](rules/MSBuild/Packaging/Proj0211.md)
* [**Proj0212** Define the project icon file explicitly](rules/MSBuild/Packaging/Proj0212.md)
* [**Proj0213** Define the project icon URL explicitly](rules/MSBuild/Packaging/Proj0213.md)
* [**Proj0214** Define the NuGet project ID explicitly](rules/MSBuild/Packaging/Proj0214.md)
* [**Proj0215** Provide a compliant NuGet package icon](rules/MSBuild/Packaging/Proj0215.md)
* [**Proj0216** Define the product name explicitly](rules/MSBuild/Packaging/Proj0216.md)
* [**Proj0217** Define requiring license acceptance explicitly](rules/MSBuild/Packaging/Proj0217.md)
* [**Proj0218** Symbol package format snupkg requires debug type portable](rules/MSBuild/Packaging/Proj0218.md)
* [**Proj0240** Enable package validation](rules/MSBuild/Packaging/Proj0240.md)
* [**Proj0241** Enable package baseline validation](rules/MSBuild/Packaging/Proj0241.md)
* [**Proj0242** Generate NuGet packages conditionally](rules/MSBuild/Packaging/Proj0242.md)
* [**Proj0243** Generate software bill of materials](rules/MSBuild/Packaging/Proj0243.md)
* [**Proj0244** Generate documentation file](rules/MSBuild/Packaging/Proj0244.md)
* [**Proj0245** Don't mix Version and VersionPrefix/VersionSuffix](rules/MSBuild/Packaging/Proj0245.md)
* [**Proj0246** Define VersionPrefix if VersionSuffix is defined](rules/MSBuild/Packaging/Proj0246.md)
* [**Proj0247** Enable strict mode for package baseline validation](rules/MSBuild/Packaging/Proj0247.md)
* [**Proj0248** Enable strict mode for package runtime compatibility validation](rules/MSBuild/Packaging/Proj0248.md)
* [**Proj0249** Enable strict mode for package framework compatibility validation](rules/MSBuild/Packaging/Proj0249.md)
* [**Proj0250** Generate API compatibility suppression file](rules/MSBuild/Packaging/Proj0250.md)
* [**Proj0251** Enable API compatibility attribute checks](rules/MSBuild/Packaging/Proj0251.md)
* [**Proj0252** Enable API compatibility parameter name checks](rules/MSBuild/Packaging/Proj0252.md)
* [**Proj0600** Avoid generating packages on build if not packable](rules/MSBuild/Packaging/Proj0600.md)

### Publishing
* [**Proj0400** Define the project publishability explicitly](rules/MSBuild/Publishing/Proj0400.md)
* [**Proj0401** Only publish executables](rules/MSBuild/Publishing/Proj0401.md)

### Test projects
* [**Proj0450** Test projects should not be packable](rules/MSBuild/Test%20Projects/Proj0450.md)
* [**Proj0451** Test projects should not be publishable](rules/MSBuild/Test%20Projects/Proj0451.md)
* [**Proj0452** Test projects require Microsoft.NET.Test.Sdk](rules/MSBuild/Test%20Projects/Proj0452.md)
* [**Proj0453** Using Microsoft.NET.Test.Sdk implies a test project](rules/MSBuild/Test%20Projects/Proj0453.md)

### Licensing
* [**Proj0500** Only include packages with an explicitly defined license](rules/Licensing/Proj0500.md)
* [**Proj0501** Package only contains a deprecated license URL](rules/Licensing/Proj0501.md)
* [**Proj0502** Only include packages compliant with project license](rules/Licensing/Proj0502.md)
* [**Proj0503** Package license is unknown](rules/Licensing/Proj0503.md)
* [**Proj0504** Package license has changed](rules/Licensing/Proj0504.md)
* [**Proj0505** Third-party license registry requires include](rules/Licensing/Proj0505.md)
* [**Proj0506** Third-party license registry requires hash](rules/Licensing/Proj0506.md)
* [**Proj0507** Third-party license registry must be unconditional](rules/Licensing/Proj0507.md)
* [**Proj0508** Order third-party licenses alphabetically](rules/Licensing/Proj0508.md)

### .NET Project File Analyzers SDK
* [**Proj0700** Avoid defining &lt;Compile&gt; items in SDK project](rules/MSBuild/SDK/Proj0700.md)

### Central Package Management
* [**Proj0800** Configure Central Package Management](rules/MSBuild/Central%20Package%20Managment/Proj0800.md)
* [**Proj0801** Include 'Directory.Packages.props'](rules/MSBuild/Central%20Package%20Managment/Proj0801.md)
* [**Proj0802** Enable Central Package Management centrally](rules/MSBuild/Central%20Package%20Managment/Proj0802.md)
* [**Proj0803** Use VersionOverride only with Central Package Management enabled](rules/MSBuild/Central%20Package%20Managment/Proj0803.md)
* [**Proj0804** Use Version only with Central Package Management not enabled](rules/MSBuild/Central%20Package%20Managment/Proj0804.md)
* [**Proj0805** Define version for PackageReference](rules/MSBuild/Central%20Package%20Managment/Proj0805.md)
* [**Proj0806** VersionOverride should change the version](rules/MSBuild/Central%20Package%20Managment/Proj0806.md)
* [**Proj0807** Use Directory.Packages.props only for Central Package Management](rules/MSBuild/Central%20Package%20Managment/Proj0807.md)
* [**Proj0808** Define global package reference only in Directory.Packages.props](rules/MSBuild/Central%20Package%20Managment/Proj0808.md)
* [**Proj0809** Global package references are meant for private assets only](rules/MSBuild/Central%20Package%20Managment/Proj0809.md)
* [**Proj0810** Remove unused package versions](rules/MSBuild/Central%20Package%20Managment/Proj0810.md)

### Analyzers
* [**Proj1000** Use the .NET project file analyzers](rules/MSBuild/Analyzers/Proj1000.md)
* [**Proj1001** Use analyzers for packages](rules/MSBuild/Analyzers/Proj1001.md)
* [**Proj1002** Use Microsoft's analyzers](rules/MSBuild/Analyzers/Proj1002.md)
* [**Proj1003** Use Sonar analyzers](rules/MSBuild/Analyzers/Proj1003.md)

### Formatting
* [**Proj1700** Indent XML](rules/Formatting/Proj1700.md)
* [**Proj1701** Use CDATA for large texts](rules/Formatting/Proj1701.md)
* [**Proj1702** Omit XML declarations](rules/Formatting/Proj1702.md)

### Other
* [**Proj1100** Avoid using Moq](rules/MSBuild/Other/Proj1100.md)
* [**Proj1101** Package references should have stable versions](rules/MSBuild/Other/Proj1101.md)
* [**Proj1102** Use Coverlet Collector or MSBuild](rules/MSBuild/Other/Proj1102.md)
* [**Proj1103** TUnit test projects must be executable](rules/MSBuild/Other/Proj1103.md)
* [**Proj1104** TUnit conflicts with Microsoft.NET.Test.Sdk](rules/MSBuild/Other/Proj1104.md)
* [**Proj1200** Exclude private assets as project file dependency](rules/MSBuild/Other/Proj1200.md)

## Resource file rules/Resources
* [**Proj2000** Embed valid resource files](rules/Resources/Proj2000.md)
* [**Proj2001** Define data in a resource file](rules/Resources/Proj2001.md)
* [**Proj2002** Sort resource file values alphabetically](rules/Resources/Proj2002.md)
* [**Proj2003** Add invariant fallback resources](rules/Resources/Proj2003.md)
* [**Proj2004** Add invariant fallback values](rules/Resources/Proj2004.md)
* [**Proj2005** Escape XML nodes of resource values](rules/Resources/Proj2005.md)
* [**Proj2100** Indent RESX](rules/Resources/Proj2100.md)

## Generic
* [**Proj3000** Only use UTF-8 without BOM](rules/Generic/Proj3000.md)
* [**Proj3001** Track uses of "TODO" tags](rules/Generic/Proj3001.md)
* [**Proj3002** Remove commented-out code](rules/Generic/Proj3002.md)

## INI
* [**Proj4000** Invalid INI file](rules/INI/Proj4000.md)
* [**Proj4001** Invalid INI header](rules/INI/Proj4001.md)
* [**Proj4002** Invalid INI key-value pair](rules/INI/Proj4002.md)
* [**Proj4010** Sections should contain at least one key-value pair](rules/INI/Proj4010.md)

## .editorconfig
* [**Proj4050** Header must be a GLOB](rules/Editorconfig/Proj4050.md)
* [**Proj4051** Use equals sign for key-value assignments](rules/Editorconfig/Proj4051.md)

## Sonar integration
By default, results by .NET project file analyzers are not added to Sonar's reporting. Read [here](sonar-integration.md) how to configure this correctly.

## License
.NET project file analyzers is licensed under [MIT](../LICENSE.MD).
