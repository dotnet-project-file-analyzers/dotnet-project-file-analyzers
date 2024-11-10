using DotNetProjectFile.Analyzers.TestTools;

namespace Benchmarks;

public class Levenshtein_distance
{
    private readonly string[] Words;
    private readonly string Word;
    private readonly Fastenshtein.Levenshtein[] Cached_Fasten;
    private readonly DotNetProjectFile.Text.Levenshtein[] Cached;

    public Levenshtein_distance()
    {
        var rnd = new Random(42);
        Words = Enumerable
            .Range(0, 100)
            .Select(_ => rnd.NextWord(16))
            .ToArray();

        Word = rnd.NextWord(16);

        Cached_Fasten = Words.Select(w => new Fastenshtein.Levenshtein(w)).ToArray();
        Cached = Words.Select(w => new DotNetProjectFile.Text.Levenshtein(w)).ToArray();
    }

    [Benchmark(Baseline = true)]
    public int NonOptimized()
    {
        var sum = 0;
        foreach (var word in Words)
        {
            sum += DotNetProjectFile.Analyzers.TestTools.References.Levenshtein.Distance(Word, word);
        }
        return sum;
    }

    [Benchmark]
    public int Implementation()
    {
        var sum = 0;
        foreach (var word in Cached)
        {
            sum += word.DistanceFrom(Word);
        }
        return sum;

    }

    [Benchmark]
    public int Optimized()
    {
        var sum = 0;
        foreach (var word in Words)
        {
            sum += DotNetProjectFile.Analyzers.TestTools.References.Levenshtein.Optimized(Word, word);
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
        foreach (var word in Cached_Fasten)
        {
            sum += word.DistanceFrom(Word);
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
