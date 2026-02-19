using CodeAnalysis.TestTools.Contexts;
using System.Diagnostics.CodeAnalysis;
using System.IO;

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
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineProject(
        this DiagnosticAnalyzer analyzer,
        Language language,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
    {
        if (language.IsRoslynBased)
        {
            return analyzer.ForInlineProject($"inline{language.ProjectFileExtension}", content);
        }
        else
        {
            return analyzer
                .ForInlineSdkProject()
                .WithFile($"inline/inline{language.ProjectFileExtension}", content);
        }
    }

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineSdkProject(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer.ForInlineProject(".net.csproj", content);

    /// <remarks>
    /// Includes a copy of DotNetProjectFile.Analyzers.Sdk.props.
    /// </remarks>
    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineSdkProject(
        this DiagnosticAnalyzer analyzer)
        => analyzer.ForInlineSdkProject("""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <!-- The TargetFramework may be overridden, but has a fine default. -->
                <TargetFramework>net10.0</TargetFramework>
                <LangVersion>latest</LangVersion>
                <IsPackable>false</IsPackable>
                <IsPublishable>false</IsPublishable>
              </PropertyGroup>

              <!-- We do not want to enable default items here. -->
              <PropertyGroup>
                <EnableDefaultItems>false</EnableDefaultItems>
              </PropertyGroup>

              <!-- The bin is just noise here, so move it a temp location. -->
              <PropertyGroup>
                <OutputPath>$([System.IO.Path]::GetTempPath())/.net/bin</OutputPath>
                <IntermediateOutputPath>$([System.IO.Path]::GetTempPath())/.net/obj</IntermediateOutputPath>
              </PropertyGroup>

              <ItemGroup Label="Add without showing">
                <AdditionalFiles Visible="false" Include="$(MSBuildProjectFile)" />
                <AdditionalFiles Visible="false" Include="**/*.csproj" />
                <AdditionalFiles Visible="false" Include="**/*.props" />
                <AdditionalFiles Visible="false" Include="**/*.targets" />
                <AdditionalFiles Visible="false" Include="**/*.slnx" />
                <AdditionalFiles Visible="false" Include="**/*.vbproj" />
                <AdditionalFiles Visible="false" Include="**/*.fsproj" />
                <AdditionalFiles Visible="false" Include="**/*.cblproj" />
              </ItemGroup>

              <ItemGroup Label="Exclude generated stuff">
                <AdditionalFiles Remove="**/bin/**" />
                <AdditionalFiles Remove="**/obj/**" />
              </ItemGroup>

              <ItemGroup>
                <AdditionalFiles Include=".*config" />
                <AdditionalFiles Include=".git*" />
                <AdditionalFiles Include=".github/**" />
                <AdditionalFiles Include="*.config" />
                <AdditionalFiles Include="*.ini" />
                <AdditionalFiles Include="*.json" />
                <AdditionalFiles Include="*.md" />
                <AdditionalFiles Include="*.props" />
                <AdditionalFiles Include="*.targets" />
                <AdditionalFiles Include="*.txt" />
                <AdditionalFiles Include="*.yaml" />
                <AdditionalFiles Include="*.yml" />
                <AdditionalFiles Include="props/*.props" />
                <AdditionalFiles Include="props/*.targets" />
              </ItemGroup>

              <ItemGroup>
                <None Include="**/TestResults/**" />
              </ItemGroup>

            </Project>
            
            """);

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineNuGetConfig(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer
        .ForInlineSdkProject()
        .WithFile("NuGet.config", content);

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineSlnx(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer
        .ForInlineSdkProject()
        .WithFile("inline.slnx", content);

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineCsproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer.ForInlineProject(Language.CSharp, content);

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineVbproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer.ForInlineProject(Language.VisualBasic, content);

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineFsproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer.ForInlineProject(Language.FSharp, content);

    [Pure]
    public static InlineProjectAnalyzerVerifyContextBuilder ForInlineCblproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
        => analyzer.ForInlineProject(Language.VisualCobol, content);

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
