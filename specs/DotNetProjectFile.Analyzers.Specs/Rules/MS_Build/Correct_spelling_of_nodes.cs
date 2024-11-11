using DotNetProjectFile.MsBuild;
using System.IO;

namespace Rules.MS_Build.Correct_spelling_of_nodes;

public class Reports
{
    [Test]
    public void on_spelling_errors() => new CorrectSpellingOfNodes()
        .ForProject("Misspelled.cs")
        .HasIssues(
            new Issue("Proj0031", "The node name EnableNetAnalyzers is misspelled, EnableNETAnalyzers might be intended."),
            new Issue("Proj0031", "The node name IsPublishible is misspelled, IsPublishable might be intended."),
            new Issue("Proj0031", "The node name PackageReferences is misspelled, PackageReference might be intended."),
            new Issue("Proj0031", "The node name COMPILE is misspelled, Compile might be intended."),
            new Issue("Proj0031", "The node name AdditionalFile is misspelled, AdditionalFiles might be intended."));
}

public class Guards
{
    private readonly CorrectSpellingOfNodes Analyzer = new();

    private static readonly string[] KnownNodes = LoadKnownNodes().ToArray();

    [TestCase("CompliantCSharp.cs")]
    [TestCase("CompliantCSharpPackage.cs")]
    public void Projects_without_issues(string project) => Analyzer
        .ForProject(project)
        .HasNoIssues();

    [TestCaseSource(nameof(KnownNodes))]
    public void Known_nodes(string name)
        => Analyzer.GetSuggestion(name, []).Should().BeNull();

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
