using System.Collections.Immutable;

namespace Benchmarks;

public class RunAll
{
    private ProjectAnalyzerVerifyContext? AnalyzersContext;
    private ProjectAnalyzerVerifyContext? EmptyContext;

    [GlobalSetup]
    public void Setup()
    {
        EmptyContext = new NoAnalyzers().ForProject(Files[File]);
        AnalyzersContext = EmptyContext;

        foreach (var analyzer in Analyzers)
        {
            AnalyzersContext = AnalyzersContext.Add(analyzer);
        }
    }

    [Params("Project", "CompliantCS", "CompliantVB")]
    public string File { get; set; } = string.Empty;

    [Benchmark(Baseline = true)]
    public Task<IReadOnlyCollection<Diagnostic>> no_analyzers()
       => EmptyContext!.GetDiagnosticsAsync();

    [Benchmark]
    public Task<IReadOnlyCollection<Diagnostic>> all_analyzers()
        => AnalyzersContext!.GetDiagnosticsAsync();

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


    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    private sealed class NoAnalyzers : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [];

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        }
    }
}
