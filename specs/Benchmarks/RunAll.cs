namespace Benchmarks;

public class RunAll
{
    private ProjectAnalyzerVerifyContext? Context;

    [GlobalSetup]
    public void Setup()
    {
        Context = Analyzers[0].ForProject(Files[File]);
        foreach(var analyzer in Analyzers[1..])
        {
            Context = Context.Add(analyzer);
        }
    }

    [Params("Project", "CompliantCS", "CompliantVB")]
    public string File { get; set; } = string.Empty;

    [Benchmark]
    public Task<Compilation> GetCompilation()
        => Context!.GetCompilationAsync();

    [Benchmark]
    public Task<IReadOnlyCollection<Diagnostic>> GetDiagnostics()
        => Context!.GetDiagnosticsAsync();

    private static readonly DiagnosticAnalyzer[] Analyzers =
        typeof(MsBuildProjectFileAnalyzer).Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(DiagnosticAnalyzer)))
            .Select(t => (DiagnosticAnalyzer)Activator.CreateInstance(t)!)
            .ToArray();

    private static readonly Dictionary<string, FileInfo> Files = new()
    {
        ["Project"] = new FileInfo($"../../../../../../../../../src/DotNetProjectFile.Analyzers/DotNetProjectFile.Analyzers.csproj"),
        ["CompliantCS"] = new FileInfo($"../../../../../../../../../projects/CompliantCSharp/CompliantCSharp.csproj"),
        ["CompliantVB"] = new FileInfo($"../../../../../../../../../projects/CompliantVB/CompliantVB.vbproj"),
    };
}
