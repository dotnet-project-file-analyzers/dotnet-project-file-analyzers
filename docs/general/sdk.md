---
permalink: /sdk
nav_order: 2
---

# Using .NET Project File Analyzers on Shared Files
.NET project file analyzers work by linking files to a project (most commonly
a `*.csproj` file) and hooking into Roslyn when that project is built. However,
for files that are not linked to any single project—such as solution files,
shared configuration files, or repository, this approach doesn't work.

The `.net.csproj` file provides a solution: it is a dedicated proxy project that
analyzes all such unlinked files in your repository.

## How the `.net.csproj` file Works
The `.net.csproj` file is a special proxy project that enables analysis of
repository-level files. It automatically includes The analyzer automatically
detects and analyzes compatible files in the project's directory tree (such as
`.csproj`, `.slnx`, `.editorconfig`, `NuGet.config`, etc.), along with any
files explicitly included.

Placement of the `.net.csproj` file should be a parent directory common to all
projects you want to analyze. In a [monorepo](https://en.wikipedia.org/wiki/Monorepo)
this most likely same directory of your solution file, otherwise the root of
your repo is the most logical choice.

The `.net.cspoj` file is configured to have no build output and not to
automatically include `<Compile>` items.

`.net.csproj` includes top level files and as such provides a solid alternative
to a `Solution items` folder.

## .net.csproj
A `.net.csproj` project file looks like this:

``` xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference PrivateAssets="all" Include="DotNetProjectFile.Analyzers" Version="1.16.1" />
  </ItemGroup>

</Project>
```

*Download this example [`.net.csproj`](.net.csproj)*


## Central Package Management
It is advised to add the reference in the `Directory.Build.props` file, or
`Directory.Packages.props` when [Central Package Management](rules/Proj0800.md)
is enabled. In the latter case using a `<GlobalPackageReference>`:

``` xml
<ItemGroup Label="Analyzers">
  <GlobalPackageReference Include="DotNetProjectFile.Analyzers" Version="1.15.2" />
</ItemGroup>
```

In that case the `.net.csproj` can be as small as this:
``` xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

</Project>
```
