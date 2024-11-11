using DotNetProjectFile.MsBuild;
using System.IO;

namespace Rules.MS_Build.Correct_spelling_of_nodes;

public class Guards
{
    private readonly CorrectSpellingOfNodes Analyzer = new();

    private static readonly string[] KnownNodes = LoadKnownNodes().ToArray();

    [TestCaseSource(nameof(KnownNodes))]
    public void Known_nodes(string name)
        => Analyzer.GetSuggestion(name).Should().BeNull();

    private static IEnumerable<string> LoadKnownNodes()
    {
        using var stream = new StreamReader("../../../Rules/MS_Build/Correct_spelling_of_nodes.txt");

        while(stream.ReadLine() is string line)
        {
            if (!Node.Factory.KnownNodes.Contains(line))
            {
                yield return line;
            }
        }
    }
}
