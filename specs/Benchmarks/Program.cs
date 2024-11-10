using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<Levenshtein_distance>();
    }

    public static void Secondary(string[] args)
    {
        BenchmarkRunner.Run<Globs>();
        BenchmarkRunner.Run<GitIgnoreFile>();
        BenchmarkRunner.Run<RunAll>();
    }
}
