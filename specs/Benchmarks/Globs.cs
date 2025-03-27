using DotNetProjectFile.Text;

namespace Benchmarks;

public class Globs
{
    public class Matching
    {
        private static readonly Glob ProjFile = Glob.TryParse("**.{csproj,vbproj,props,xml}")!.Value;
        private static readonly GlobExpressions.Glob Referense = new("**.{csproj,vbproj,props,xml}", GlobExpressions.GlobOptions.Compiled);

        [Params(
            "C://code/project/DotNetProjectFile.Analyzers.csproj",
            "project/DotNetProjectFile.Analyzers.xml",
            "no-match")]
        public string Paths { get; set; } = string.Empty;

        [Benchmark]
        public bool DotNetProjectFile_IO_Glob()
            => ProjFile.IsMatch(Paths);

        [Benchmark]
        public bool GlobExpressions_Glob()
            => Referense.IsMatch(Paths);
    }

    public class Parsing
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
}
