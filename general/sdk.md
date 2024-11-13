# .NET Project File Analyzers SDK
.NET Project File Analyzers ships with its own SDK. This allows files shared by
multiple projects to be analyzed. It applies a *trick* also used by the
[Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk);
providing both a custom `.props` as a `.targets` MS Build file.

## .net.csproj
So how does it work? At the root level of your solution (most likely the
directory conting the [Git](https://en.wikipedia.org/wiki/Git) repo), you
 create a C# Project file with the name `.net.csproj`.

``` XML
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
   <PackageReference Include="DotNetProjectFile.Analyzers.Sdk" Version="*" PrivateAssets="all" />
  </ItemGroup>
  
</Project>
```

Most files in the directory of this SDK project will be Included automatically.
This includes configuration, text, and markdown files. It will not contain any
`<Compile>` items unless explictly added. The SDK project is not intended to
contain `<Compile>` items, and the binary ouptut is hidden for that reason.

All automatically included files and files added as `<AdditionalFiles>` are
analyzed by the appropriate .NET Project File Analyzers: those analyzers are
also inlcuded automatically.

The SDK project can - on top of the analysis - also act as a replacement of
the `Solution Items` folder (and other folders) a lot of `.sln` solution files
contain. This should improve the maintainabillity of the `.sln` solution too.
