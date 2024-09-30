using DotNetProjectFile.IO.Globbing;

namespace Benchmarks;

public class Globs
{
    [Params("*.*", "*.{cs,vb,ts}", "[Dd]ebug", "*.{csproj,vbproj,props,xml}")]
    public string Expression { get; set; } = string.Empty;

    [Benchmark]
    public Segement DotNetProjectFile_IO()
        => GlobParser.TryParse(Expression);

    [Benchmark]
    public GlobExpressions.Glob GlobExpressions_Glob() => new(Expression, GlobExpressions.GlobOptions.Compiled);

    [Benchmark]
    public DotNet.Globbing.Glob DotNet_Globbing()
        => DotNet.Globbing.Glob.Parse(Expression);
}
