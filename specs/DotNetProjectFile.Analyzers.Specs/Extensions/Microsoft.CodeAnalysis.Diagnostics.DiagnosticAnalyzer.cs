using CodeAnalysis.TestTools.Contexts;
using Specs.TestTools;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;

namespace Microsoft.CodeAnalysis.Diagnostics;

internal static class ProjectFileAnalyzersDiagnosticAnalyzerExtensions
{
    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineProject(
        this DiagnosticAnalyzer analyzer,
        string fileName,
        string content)
        => new(analyzer, fileName, content);

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineSdkProject(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer.ForInlineProject(".net.csproj", content);

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineSdkProject(
        this DiagnosticAnalyzer analyzer)
        => analyzer.ForInlineSdkProject("""
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>

  <ItemGroup>
    <AdditionalFiles Include="**/*.slnx" />
    <AdditionalFiles Include="**/*.csproj" />
    <AdditionalFiles Include="**/*.vbproj" />
    <AdditionalFiles Include="**/*.fsproj" />
    <AdditionalFiles Include="**/*.cblproj" />
    <AdditionalFiles Include="**/*.props" />
    <AdditionalFiles Include="**/*.targets" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="DotNetProjectFile.Analyzers.Sdk" Version="*" PrivateAssets="all" ExcludeAssets="runtime" />
  </ItemGroup>

</Project>
""");

    [Pure]
    public static ProjectAnalyzerVerifyContext ForInlineSlnx(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer
        .ForInlineSdkProject()
        .WithFile("inline.slnx", content)
        .Build();

    [Pure]
    public static ProjectAnalyzerVerifyContext ForInlineCsproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer.ForInlineProject("inline.csproj", content).Build();

    [Pure]
    public static ProjectAnalyzerVerifyContext ForInlineVbproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer.ForInlineProject("inline.vbproj", content).Build();

    [Pure]
    public static ProjectAnalyzerVerifyContext ForInlineFsproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer
        .ForInlineSdkProject()
        .WithFile("inline/inline.fsproj", content)
        .Build();

    [Pure]
    public static ProjectAnalyzerVerifyContext ForInlineCblproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer
        .ForInlineSdkProject()
        .WithFile("inline/inline.cblproj", content)
        .Build();

    [Pure]
    public static ProjectAnalyzerVerifyContext ForProject(this DiagnosticAnalyzer analyzer, string fileName)
    {
        var name = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        if (extension == ".sdk")
        {
            return analyzer.ForSdkProject(name);
        }
        else
        {
            var file = new FileInfo($"../../../../../projects/{name}/{fileName}proj");
            return ForTestProject(analyzer, file);
        }
    }

    [Pure]
    public static ProjectAnalyzerVerifyContext ForSdkProject(this DiagnosticAnalyzer analyzer, string name)
    {
        var directory = new DirectoryInfo($"../../../../../projects/{name}");

        var file = directory
            .EnumerateFiles()
            .First(f => ".net.csproj".Equals(f.Name, StringComparison.OrdinalIgnoreCase));

        return ForTestProject(analyzer, file);
    }

    [Pure]
    public static ProjectAnalyzerVerifyContext ForTestProject(this DiagnosticAnalyzer analyzer, FileInfo file)
    {
        var project = CachedProjectLoader.Load(file);

        if (project.MetadataReferences.Count == 0)
        {
            throw new InvalidOperationException("Project could not be compiled.");
        }

        var context = new ProjectAnalyzerVerifyContext(project).Add(analyzer);

        return context with
        {
            IgnoredDiagnostics = DiagnosticIds.Empty.AddRange(
                "BC50001", // Unused import statement.

                "CS1701", // Assuming assembly reference.
                "CS8019", // Unnecessary using directive.
                "CS8933", // The using directive appeared previously as global using

                "??????"
            ),
        };
    }
}
