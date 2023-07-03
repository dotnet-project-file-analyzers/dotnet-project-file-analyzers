using CodeAnalysis.TestTools;
using CodeAnalysis.TestTools.Contexts;

namespace Microsoft.CodeAnalysis.Diagnostics;

internal static class DiagnosticAnalyzerExtensions
{
    public static ProjectAnalyzerVerifyContext ForProject(this DiagnosticAnalyzer analyzer, string fileName)
    {
        var name = Path.GetFileNameWithoutExtension(fileName);
        var file = new FileInfo($"../../../../../projects/{name}/{fileName}proj");
        var context = analyzer.ForProject(file);

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
