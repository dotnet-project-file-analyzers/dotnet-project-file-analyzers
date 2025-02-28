using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Licensing.Hashing>();
    }

    public static void All()
    {
        BenchmarkRunner.Run<IniFile>();
        BenchmarkRunner.Run<GitIgnoreFile>();
        BenchmarkRunner.Run<Globs.Matching>();
        BenchmarkRunner.Run<Globs.Parsing>();
        BenchmarkRunner.Run<GrammrParsing>();
        BenchmarkRunner.Run<Licensing.PrepareText>();
        BenchmarkRunner.Run<Licensing.Hashing>();
        BenchmarkRunner.Run<RunAll>();
    }
}
