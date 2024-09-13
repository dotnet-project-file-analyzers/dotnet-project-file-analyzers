using System.IO;

namespace Rules.All;

public class Guards
{
    [Test]
    public void DotNetProjectFile_Analyzers_project()
    {
        var all = Analyzers.ToArray();
        var context = all[0].ForProject(new FileInfo($"../../../../../src/DotNetProjectFile.Analyzers/DotNetProjectFile.Analyzers.csproj"))
            with
        {
            IgnoredDiagnostics = DiagnosticIds.Empty.AddRange(
                "CS8019", // Unnecessary using directive.
                "CS8933", // The using directive appeared previously as global using
                "??????")
        };

        foreach (var analyzer in all)
        {
            context = context.Add(analyzer);
        }

        context.HasNoIssues();
    }

    private static IEnumerable<DiagnosticAnalyzer> Analyzers
       => typeof(MsBuildProjectFileAnalyzer).Assembly
       .GetTypes()
       .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(DiagnosticAnalyzer)))
       .Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!)
       .Where(a => a.SupportedDiagnostics[0].IsEnabledByDefault);
}
