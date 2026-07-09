using Bench;
using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main()
        => BenchmarkRunner.Run<IniFiles>();

    public static void All()
    {
        BenchmarkRunner.Run<GitIgnoreFiles>();
        BenchmarkRunner.Run<IniFiles>();
        BenchmarkRunner.Run<Globs.Matching>();
        BenchmarkRunner.Run<Globs.Parsing>();
        BenchmarkRunner.Run<Licensing.PrepareText>();
        BenchmarkRunner.Run<Licensing.Hashing>();
        BenchmarkRunner.Run<RunAll>();
    }
}
