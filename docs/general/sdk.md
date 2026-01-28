---
permalink: /sdk
nav_order: 2
---

# .NET Project File Analyzers SDK
[![DotNetProjectFile.Analyzers.Sdk](https://img.shields.io/nuget/v/DotNetProjectFile.Analyzers.Sdk)![DotNetProjectFile.Analyzers](https://img.shields.io/nuget/dt/DotNetProjectFile.Analyzers.Sdk)](https://www.nuget.org/packages/DotNetProjectFile.Analyzers.Sdk)
.NET Project File Analyzers ships with its own SDK. This allows files shared by
multiple projects to be analyzed. It applies a *trick* also used by the
[Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk);
providing both a custom `.props` and a `.targets` MS Build file.

## .net.csproj
So how does it work? At the root level of your solution (most likely the
root directory of the [Git](https://en.wikipedia.org/wiki/Git) repo), you
create a C# Project file with the name `.net.csproj`.

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

## How does it work?
By adding the `.net.csproj` to your build (most likely by adding the project to
a `.slnx`/`.sln` solution file) the analyzers can both detect files (like
`.slnx` files, and the `NuGet.config`) that are not directly bound to a single
proj file, and analyze them once (via the `.net.csproj`).

Most files in the directory of this SDK project will be included automatically.
This includes configuration, text, and markdown files. It will not contain any
`<Compile>` items unless explicitly added. The SDK project is not intended to
contain `<Compile>` items, and the binary output is hidden for that reason.

All automatically included files and files added as `<AdditionalFiles>` are
analyzed by the appropriate .NET Project File Analyzers.

Those analyzers can be included with:

``` xml
<ItemGroup>
  <PackageReference Include="DotNetProjectFile.Analyzers" Version="*" PrivateAssets="all" />
</ItemGroup>
```

This can be in the `.net.csproj` file, but it is advised to do this in the
 `Directory.Build.props` file instead.

The SDK project can - on top of the analysis - also act as a replacement of
the `Solution Items` folder (and other folders) that contain a lot of
solution files. This should improve the maintainabillity of the `.slnx`
solution too.
