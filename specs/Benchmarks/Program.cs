using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<GrammrParsing>();
    }

    public static void All()
    {
        BenchmarkRunner.Run<IniFile>();
        BenchmarkRunner.Run<GitIgnoreFile>();
        BenchmarkRunner.Run<Globs>();
        BenchmarkRunner.Run<GrammrParsing>();
        BenchmarkRunner.Run<RunAll>();
    }
}
