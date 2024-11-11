using DotNetProjectFile.Text;

namespace Text.Levenshtein_distance_specs;

public class Gets
{
    [TestCase("ant", "aunt")]
    [TestCase("fast", "cats")]
    [TestCase("Elemar", "Vilmar")]
    [TestCase("kitten", "sitting")]
    [TestCase("GlobalAnalyzerConfigFiles", "GlobalAnalyzersConfig")]
    public void distance_between(string word, string other)
    {
        var lev = new Levenshtein(word);
        lev.DistanceFrom(other).Should().Be(Benchmarks.References.Levenshtein.Distance(word, other));
    }
}
