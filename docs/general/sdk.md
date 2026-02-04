---
permalink: /sdk
nav_order: 2
---

# .NET project file analyzers SDK
[![DotNetProjectFile.Analyzers.Sdk](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers.Sdk)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers.Sdk)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers.Sdk)
.NET project file analyzers ships with its own SDK.

## Why the .NET project file analyzers SDK?
The .NET project file analyzers work by bounding files to a project (most
commonly a `*.csproj` file), and hook on to Roslyn when that project is built.
For files that are shared amongst multiple projects that does not work.

## How does it work?
To analyse those files that are shared a trick is needed: A special project
named `.net.csproj`. It obtains it's powers because it is a little different
than normal projects:

1. The `.net.csproj` file is typically placed in a parent directory common to
   all other projects. This could be the root of your repo, the same directory
   as your solution(s), or somewhere in between. Placement really depends on
   which (sub) directories should be scanned, experiment a little to see what
   works for you. The needs of a large [monorepo](https://en.wikipedia.org/wiki/Monorepo) with many solutions and projects differ from those of a small repository with a single solution and just a few projects.
2. The `.net.csproj` project has a PackageReference to [![DotNetProjectFile.Analyzers.Sdk](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers.Sdk)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers.Sdk)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers.Sdk).
   It automatically includes files it can analyse. Those, and files included as
   `<AdditionalFiles>` are analyzed by the appropriate .NET project file analyzers.
 
 Although the `.net.csproj` is not supposed to contain `<Compile>` items, the
 compiler still generates output. Since that output is useless, it is hidden.

`.net.csproj` includes top level files and as such provides a solid alternative to "Solution items".

## .net.csproj
A `.net.csproj` MS Build file, is a project file that looks like this:

``` xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
   <PackageReference Include="DotNetProjectFile.Analyzers.Sdk" Version="*" PrivateAssets="all" />
  </ItemGroup>
  
</Project>
```
*Download this example [`.net.csproj`](./.net.csproj)*

## Enable analyzers for .net.csproj
The .NET project file analyzers can be included to the `.net.csproj` by adding

``` xml
<ItemGroup>
  <PackageReference Include="DotNetProjectFile.Analyzers" Version="*" PrivateAssets="all" />
</ItemGroup>
```

However, it is advised to add the reference in the `Directory.Build.props` file, or
`Directory.Packages.props`. In the latter case using a `<GlobalPackageReference>`:

``` xml
<ItemGroup Label="Analyzers">
  <GlobalPackageReference Include="DotNetProjectFile.Analyzers" Version="1.8.3" />
</ItemGroup>
```
