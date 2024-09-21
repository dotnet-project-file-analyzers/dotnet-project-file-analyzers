using DotNetProjectFile.IO;

namespace Benchmarks;

public class Globs
{
    [Params("*.cs", "*{.vbpoj,*.csproj}", "**/[ab].??proj")]
    public string Expression { get; set; } = string.Empty;

    [Benchmark]
    public Glob? Parse()
        => Glob.TryParse(Expression);
}
