using CodeAnalysis.TestTools.Contexts;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace Microsoft.CodeAnalysis.Diagnostics;

internal static class DiagnosticAnalyzerExtensions
{
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

    private static readonly ConcurrentDictionary<string, Project> projectCache = new();
    private static readonly Lock projectCacheLock = new();

    private static Project LoadProject(FileInfo file)
    {
        var key = file.FullName;

        if (projectCache.TryGetValue(key, out var project))
        {
            return project;
        }

        lock (projectCacheLock)
        {
            if (!projectCache.TryGetValue(key, out project))
            {
                project = ProjectLoader.Load(file);
                projectCache[key] = project;
            }
        }

        return project;
    }

    [Pure]
    private static ProjectAnalyzerVerifyContext ForTestProject(this DiagnosticAnalyzer analyzer, FileInfo file)
    {
        var project = LoadProject(file);
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
