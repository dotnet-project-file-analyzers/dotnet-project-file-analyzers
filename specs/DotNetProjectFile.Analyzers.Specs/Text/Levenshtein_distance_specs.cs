using DotNetProjectFile.Text;
using Reference = DotNetProjectFile.Analyzers.TestTools.References.Levenshtein;

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
        lev.DistanceFrom(other).Should().Be(Reference.Distance(word, other));
    }
}
