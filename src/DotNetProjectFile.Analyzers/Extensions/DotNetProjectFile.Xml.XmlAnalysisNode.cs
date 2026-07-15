namespace DotNetProjectFile.Xml;

public static class XmlAnalysisNodeExtensions
{
    extension(XmlAnalysisNode node)
    {
        public IEnumerable<XmlAnalysisNode> DescendantsAndSelf()
        {
            yield return node;

            foreach (var child in node.Children().SelectMany(n => n.DescendantsAndSelf()))
            {
                yield return child;
            }
        }
    }
}
