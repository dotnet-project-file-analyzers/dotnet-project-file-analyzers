using Benchmarks.Tools;

namespace Benchmarks;

public class Levenshtein_distance
{
    private readonly string[] Words;
    private readonly string Word;

    public Levenshtein_distance()
    {
        var rnd = new Random(42);
        Words = Enumerable
            .Range(0, 100)
            .Select(_ => rnd.NextWord(16))
            .ToArray();

        Word = rnd.NextWord(16);
    }

    [Benchmark(Baseline = true)]
    public int NonOptimized()
    {
        var sum = 0;
        foreach (var word in Words)
        {
            sum += References.Levenshtein.Distance(Word, word);
        }
        return sum;
    }

    [Benchmark]
    public int Implementation()
    {
        var sum = 0;
        var cached = new DotNetProjectFile.Text.Levenshtein(Word);

        foreach (var word in Words)
        {
            sum += cached.DistanceFrom(word);
        }
        return sum;

    }

    [Benchmark]
    public int Optimized()
    {
        var sum = 0;
        foreach (var word in Words)
        {
            sum += References.Levenshtein.Optimized(Word, word);
        }
        return sum;
    }

    [Benchmark]
    public int Fastenshtein_()
    {
        var sum = 0;
        foreach (var word in Words)
        {
            sum += Fastenshtein.Levenshtein.Distance(Word, word);
        }
        return sum;
    }

    [Benchmark]
    public int Fastenshtein_Cached()
    {
        var sum = 0;
        var cached = new Fastenshtein.Levenshtein(Word);
        foreach (var word in Words)
        {
            sum += cached.DistanceFrom(word);
        }
        return sum;
    }

    [Benchmark]
    public int Quickenshtein_()
    {
        var sum = 0;
        foreach (var word in Words)
        {
            sum += Quickenshtein.Levenshtein.GetDistance(Word, word);
        }
        return sum;
    }

    [Benchmark]
    public int NinjaNye_()
    {
        var sum = 0;
        foreach (var word in Words)
        {
            sum += NinjaNye.SearchExtensions.Levenshtein.LevenshteinProcessor.LevenshteinDistance(Word, word);
        }
        return sum;
    }
}
