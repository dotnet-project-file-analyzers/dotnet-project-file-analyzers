using CodeAnalysis.TestTools.Contexts;
using Specs.TestTools;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;

namespace Microsoft.CodeAnalysis.Diagnostics;

internal static class DiagnosticAnalyzerExtensions
{
    [Pure]
    public static ProjectAnalyzerVerifyContext ForInlineCsproj(
        this DiagnosticAnalyzer analyzer,
        [StringSyntax(StringSyntaxAttribute.Xml)] string content)
    {
        content = content.Trim();
#pragma warning disable RS1035 // FP: Not an analyzer.
        var tempDir = Path.Combine(Path.GetTempPath(), "dotnet-project-file-analyzer/tests");
        var hash = GetHash(content);
        var dir = Path.Combine(tempDir, hash);
        Directory.CreateDirectory(dir);
        var fileName = Path.Combine(dir, $"{hash}.csproj");
        var file = new FileInfo(fileName);

        if (File.Exists(fileName))
        {
            var fileContent = File.ReadAllText(fileName);
            var fileHash = GetHash(fileContent);

            if (hash == fileHash)
            {
                return ForTestProject(analyzer, file);
            }
        }

        File.WriteAllText(fileName, content);
        return ForTestProject(analyzer, file);

#pragma warning restore RS1035
    }

    private static string GetHash(string content)
    {
        var input = Encoding.UTF8.GetBytes(content);
        using var md5 = MD5.Create();
        var output = md5.ComputeHash(input);
        var str = Convert.ToHexString(output);
        return str;
    }

    [Pure]
    public static ProjectAnalyzerVerifyContext ForProject(this DiagnosticAnalyzer analyzer, string fileName)
    {
        var name = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        if (extension == ".sdk")
        {
            return analyzer.ForSDkProject(name);
        }
        else
        {
            var file = new FileInfo($"../../../../../projects/{name}/{fileName}proj");
            return ForTestProject(analyzer, file);
        }
    }

    [Pure]
    public static ProjectAnalyzerVerifyContext ForSDkProject(this DiagnosticAnalyzer analyzer, string name)
    {
        var file = new FileInfo($"../../../../../projects/{name}/.net.csproj");
        return ForTestProject(analyzer, file);
    }

    [Pure]
    private static ProjectAnalyzerVerifyContext ForTestProject(this DiagnosticAnalyzer analyzer, FileInfo file)
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
