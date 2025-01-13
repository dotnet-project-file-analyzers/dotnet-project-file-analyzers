# Configuration of .NET project file analyzers
Although all **.NET project file analyzers** should work like a charm,
there might be reasons to do some adjustments: disabling a specific rule or
changing its severity. The good news is: this can be done.

## Editor configuration
Most (but not all) C# and VB.NET rules can be configured in the `.editorconfig`
file. Unfortunately, changing the severity (and other configuration) of rules
in the `.editorconfig` is [**NOT** supported by MS Build](https://github.com/dotnet/roslyn/issues/37876).

## Global analyzer config
It is also possible to configure rules using a [Global AnalyzerConfig](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/configuration-files#global-analyzerconfig)
`.globalconfig` file located in the same directory as the project file or in
one of its (grand)parent directories. The following `.globalconfig` file will
disable rule `Proj0010` and raise `Proj0011` to error level:

``` ini
is_global = true

dotnet_diagnostic.Proj0010.severity = none  # Define the <OutputType> node explicitly.
dotnet_diagnostic.Proj0011.severity = error # Property <{0}> has been already defined.
```

## Analyzer INI file
It is also possible to define project specific preferences as you would have done in
an `.globalconfig` file, using `<GlobalAnalyzerConfigFiles>`:

``` xml
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <GlobalAnalyzerConfigFiles Include="../../analyzers-config.ini" />
  </ItemGroup>
</Project>
```

It is recommended to use the `.ini` extension for such a file, as it helps
with syntax highlighting. A custom config file could look like this:

``` ini
is_global = false

dotnet_diagnostic.Proj0002.severity = error # Upgrade legacy MS Build project files
dotnet_diagnostic.Proj0010.severity = none  # Define OutputType explicitly
```

## Disable rules using <NoWarn>
It is possible to disable warnings through the `<NoWarn>` tags inside a `<PropertyGroup>`
tag inside your `.csproj` (or `.props`) file.

An example of disabling rules `Proj0010` and `Proj0011` through the `.csproj` file:

``` xml
<Project Sdk="Microsoft.NET.Sdk">
  
  <PropertyGroup>
    <NoWarn>Proj0010;Proj0011</NoWarn>
  </PropertyGroup>
  
</PropertyGroup>
```

## Suppress specific warnings
Adopted from C-style languages, it is possible to suppress
individual violations and/or [false positives](https://en.wikipedia.org/wiki/False_positives_and_false_negatives).
In a MS Build project file this would look like:

``` xml
<Project Sdk="Microsoft.NET.Sdk">

  <!-- #pragma warning disable Proj0008 -->

  <ItemGroup>
    <Folder Include="First" />
  </ItemGroup>
  
  <!-- #pragma warning restore Proj0008-->

</Project>
```

It is worth to point out that the `#pragma warning restore` is optional.
