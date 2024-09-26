namespace DotNetProjectFile.Xml;

public static class XmlAnalysisNodeExtensions
{
    public static IEnumerable<XmlAnalysisNode> DescendantsAndSelf(this XmlAnalysisNode node)
    {
        yield return node;

        foreach (var child in node.Children().SelectMany(n => n.DescendantsAndSelf()))
        {
            yield return child;
        }
    }
}
