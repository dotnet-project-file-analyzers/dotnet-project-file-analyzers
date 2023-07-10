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

        foreach (var analyzer in all[1..])
        {
            context = context.Add(analyzer);
        }

        // Do not reference project to itself.
        context.HasIssue(new Issue("Proj1000", "Use the .NET project file analyzers."));
    }

    private static IEnumerable<DiagnosticAnalyzer> Analyzers
       => typeof(MsBuildProjectFileAnalyzer).Assembly
       .GetTypes()
       .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(DiagnosticAnalyzer)))
       .Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!);
}
