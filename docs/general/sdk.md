---
permalink: /sdk
nav_order: 2
---

# .NET Project File Analyzers SDK
The .NET Project File Analyzers ships with its own SDK.

## Why the .NET Project File Analyzers SDK?
.NET project file analyzers work by linking files to a project (most commonly
a `*.csproj` file), and hook on to Roslyn when that project is built.

For files that can not be linked to a single project to build, this approach
does not work, and that is where the `.net.csproj` project comes in handy.

## How does it work?
The `.net.csproj` acts as proxy for all files not linked to a single project.
It obtains its powers because it is a little different from normal projects:

1. The `.net.csproj` file is typically placed in a parent directory common to
   all other projects. This could be the root of your repo, the same directory
   as your solution(s), or somewhere in between. Placement really depends on
   which (sub) directories should be scanned, experiment a little to see what
   works for you. The needs of a large [monorepo](https://en.wikipedia.org/wiki/Monorepo)
   with many solutions and projects differ from those of a small repository
   with a single solution and just a few projects.
2. The `.net.csproj` project has a PackageReference to [![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers).
   It automatically includes files it can analyze. Those, and files included as
   `<AdditionalFiles>` are analyzed by the appropriate .NET project file
   analyzers.
3. The build output has been disabled, and adding `<Compile>` items to the
   `.net.csproj` will not result in any dll or executable.

`.net.csproj` includes top level files and as such provides a solid alternative
to a `Solution items` folder.

## .net.csproj
A `.net.csproj` project file looks like this:

``` xml
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
   <PackageReference Include="DotNetProjectFile.Analyzers" Version="*" PrivateAssets="all" />
  </ItemGroup>
  
</Project>
```
*Download this example [`.net.csproj`](./.net.csproj)*

## Central Package Management
It is advised to add the reference in the `Directory.Build.props` file, or
`Directory.Packages.props` when [rules/Proj0800.md](Central Package Management)
is enabled. In the latter case using a `<GlobalPackageReference>`:

``` xml
<ItemGroup Label="Analyzers">
  <GlobalPackageReference Include="DotNetProjectFile.Analyzers" Version="1.12.0" />
</ItemGroup>
```

In that case the `.net.csproj` can be a small as this:
``` xml
<Project Sdk="Microsoft.NET.Sdk" />
```
