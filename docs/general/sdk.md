---
permalink: /sdk
nav_order: 2
---

# .NET project file analyzers SDK
[![DotNetProjectFile.Analyzers.Sdk](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers.Sdk)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers.Sdk)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers.Sdk)
.NET Project File Analyzers ships with its own SDK. This allows files shared by
multiple projects to be analyzed. It applies a *trick* also used by the
[Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk);
providing both a custom `.props` and a `.targets` MS Build file.

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

"WDue to the `DotNetProjectFile.Analyzers.Sdk` included, the `.net.csproj`
includes a set of files that can be analyzed (such as `*.fsharp`, `*.slnx`,
`NuGet.config`, and others). It does this by scanning the file system
recursively.

It will not contain any `<Compile>` items unless explicitly added. The SDK
project is not intended to contain `<Compile>` items, and the binary output is
hidden for that reason.

All automatically included files and files added as `<AdditionalFiles>` are
analyzed by the appropriate .NET project file analyzers.

Where to put the `.net.csproj` file therefore depends on which (sub) directories
should be scanned. In the most common scenario one `.net.csproj` is placed in
root directory of the repository, and included in the `.slnx`/`.sln` solution
file that is used to build the project in the pipe line.

In a [monorepo](https://en.wikipedia.org/wiki/Monorepo) scenario it is more common
to have a seperate `.net.csproj` per seperate build.

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

The SDK project can - on top of the analysis - also act as a replacement of
the `Solution Items` folder (and other folders) that contain a lot of
solution files. This should improve the maintainabillity of the `.slnx`
solution too.
