using DotNetProjectFile.IO;

namespace Benchmarks;

public class Globs
{
    [Params("*.*", "*.{cs,vb,ts}", "[Dd]ebug", "*.{csproj,vbproj,props,xml}")]
    public string Expression { get; set; } = string.Empty;

    [Benchmark]
    public Glob? DotNetProjectFile_IO_Glob()
        => Glob.TryParse(Expression);

    [Benchmark]
    public GlobExpressions.Glob GlobExpressions_Glob() => new(Expression, GlobExpressions.GlobOptions.Compiled);

    [Benchmark]
    public DotNet.Globbing.Glob DotNet_Globbing()
        => DotNet.Globbing.Glob.Parse(Expression);
}
