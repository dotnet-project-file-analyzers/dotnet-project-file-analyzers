using BenchmarkDotNet.Running;

namespace Benchmarks;

public static class Program
{
    public static void Main()
        => BenchmarkRunner.Run<Licensing.Hashing>();

    public static void All()
    {
        BenchmarkRunner.Run<Globs.Matching>();
        BenchmarkRunner.Run<Globs.Parsing>();
        BenchmarkRunner.Run<Licensing.PrepareText>();
        BenchmarkRunner.Run<Licensing.Hashing>();
        BenchmarkRunner.Run<RunAll>();
    }
}
