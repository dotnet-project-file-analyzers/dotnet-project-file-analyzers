using System.IO;

namespace Rules.All;

public class Guards
{
    [Test]
    public void DotNetProjectFile_Analyzers_project()
    {
        var all = AvailableAnalyzers.All.ToArray();
        var context = all[0].ForProject(new FileInfo($"../../../../../src/DotNetProjectFile.Analyzers/DotNetProjectFile.Analyzers.csproj"))
            with
        {
            IgnoredDiagnostics = DiagnosticIds.Empty.AddRange(
                "CS8019", // Unnecessary using directive.
                "CS8933", // The using directive appeared previously as global using

                // Default disabled
                "Proj0037",
                "Proj1000")
        };

        // Add the remaining analyzers.
        foreach (var analyzer in all[1..])
        {
            context = context.Add(analyzer);
        }
        context.HasNoIssues();
    }
}
